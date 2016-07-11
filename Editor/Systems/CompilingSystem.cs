using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using uFrame.Editor.Attributes;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Compiling.Commands;
using uFrame.Editor.Compiling.Events;
using uFrame.Editor.Configurations;
using uFrame.Editor.Core;
using uFrame.Editor.Database;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI;
using uFrame.Editor.GraphUI.ViewModels;
using uFrame.Editor.Input;
using uFrame.Editor.Menus;
using uFrame.Editor.Platform;
using uFrame.Editor.Validation;
using uFrame.IOC;
using UnityEditor;

namespace uFrame.Editor
{
    public interface ICompilingStarted
    {
        void CompilingStarted(IRepository repository);
    }
    public class CompilingSystem : DiagramPlugin
        , IToolbarQuery
        , IContextMenuQuery
        , IExecuteCommand<SaveAndCompileCommand>
        , IDataRecordInserted
        , IDataRecordRemoved
        , IDataRecordPropertyChanged

    {

        private List<IDataRecord> _changedRecrods;

        public ValidationSystem ValidationSystem
        {
            get { return Container.Resolve<ValidationSystem>(); }
        }
        public const int CURRENT_BUILD_NUMBER = 1;
        public void QueryToolbarCommands(ToolbarUI ui)
        {
            var databaseService = Container.Resolve<DatabaseService>();
            if (databaseService.CurrentConfiguration == null) return;

        
                ui.AddCommand(new ToolbarItem()
                {
                    Command = new SaveAndCompileCommand(),
                    Title = "Build",
                    Position = ToolbarPosition.Right,
                    Description = "Start code generation process. This generates C# code based on the nodes and items in the diagram."
                });
                ui.AddCommand(new ToolbarItem()
                {
                    Command = new SaveAndCompileCommand() { ForceCompileAll = true },
                    Title = "Build All",
                    Position = ToolbarPosition.Right,
                    Description = "Start code generation process. This forces all code to regenerate for everything in the database."
                });

          
            
        
       
        }

        public override void Loaded(UFrameContainer container)
        {
            base.Loaded(container);
            //#if DEMO


            //                Signal<INotify>(_=>_.NotifyWithActions("You're using the demo version of uFrame ECS.",NotificationIcon.Warning,new NotifyActionItem()
            //                {
            //                    Title = "Buy Now",
            //                    Action = ()=>
            //                    {
            //                        InvertGraphEditor.Platform.OpenLink("https://invertgamestudios.com/ecs/purchase");
            //                    }
            //                }));
            //                return;

            //#endif
        }

        public void Execute(SaveAndCompileCommand command)
        {
#if DEMO
            Signal<INotify>(x=> x.NotifyWithActions("You're using the demo version of uFrame ECS.", NotificationIcon.Warning, 
                new NotifyActionItem()
                {
                    Title = "Buy Now",
                    Action = () =>
                    {
                        InvertGraphEditor.Platform.OpenLink("https://invertgamestudios.com/ecs/purchase");
                    }
                },
                new NotifyActionItem()
                {
                    Title = "Nope, Compile It",
                    Action = () =>
                    {
                        InvertApplication.SignalEvent<ITaskHandler>(_ => { _.BeginTask(Generate(command)); });
                    }
                }
                ));
#else
            InvertApplication.SignalEvent<ITaskHandler>(_ => { _.BeginTask(Generate(command)); });

#endif

        }

        public IEnumerable<IDataRecord> GetItems(IRepository repository, bool getAll = false)
        {
            if (!getAll)
            {
                foreach (var item in repository.AllOf<IAlwaysGenerate>())
                {
                    yield return item;
                }

                foreach (var item in repository.AllOf<IGraphData>().Where(p => p.IsDirty))
                {
                    yield return item;
                    foreach (var child in item.AllGraphItems)
                    {
                        yield return child;
                    }
                }
            }
            else
            {

                foreach (var item in repository.AllOf<IDataRecord>())
                {
                    yield return item;
                }
            }


            //yield return repository.AllOf<uFrameDatabaseConfig>().FirstOrDefault();
            //var workspaceSvc = InvertApplication.Container.Resolve<WorkspaceService>();
            //foreach (var workspace in workspaceSvc.Workspaces)
            //{
            //    if (workspace.CompilationMode == CompilationMode.OnlyWhenActive &&
            //        workspace != workspaceSvc.CurrentWorkspace) continue;
            //    yield return workspace;
            //    foreach (var item in workspace.Graphs)
            //    {
            //        yield return item;
            //        foreach (var node in item.NodeItems)
            //        {
            //            yield return node;
            //            foreach (var child in node.PersistedItems)
            //                yield return child;
            //        }
            //    }
            //}
        }

        public IEnumerator Generate(SaveAndCompileCommand command)
        {


            var repository = InvertGraphEditor.Container.Resolve<IRepository>();

            var remove  = repository.AllOf<IClassNode>().Where(p => string.IsNullOrEmpty(p.Name)).ToArray();
            foreach (var item in remove)
            {
                repository.Remove(item);
            }
            //var allIds = allRecords.Select(p => p.Identifier).ToArray();

            //foreach (var item in allRecords)
            //{
            //    if (item.ForeignKeys.Where(p=>!string.IsNullOrEmpty(p)).Any(p =>!allIds.Contains(p)))
            //    {
            //        InvertApplication.Log(string.Format("{0} : {1} was removed due to invalid foreign key.", item.Identifier, item.GetType().Name));
            //        repository.Remove(item);
            //    }
            //}
            repository.Commit();
            var config = InvertGraphEditor.Container.Resolve<IGraphConfiguration>();
            var items = GetItems(repository, command.ForceCompileAll).Distinct().ToArray();
            yield return
                new TaskProgress(0f, "Validating");
            var a = ValidationSystem.ValidateNodes(items.OfType<IDiagramNode>().ToArray());
            while (a.MoveNext())
            {
                yield return a.Current;
            }
            if (ValidationSystem.ErrorNodes.SelectMany(n => n.Errors).Any(e => e.Siverity == ValidatorType.Error))
            {
                Signal<INotify>(_ => _.Notify("Please, fix all errors before compiling.", NotificationIcon.Error));
                yield break;
            }
            Signal<IUpgradeDatabase>(_ => _.UpgradeDatabase(config as uFrameDatabaseConfig));
            Signal<ICompilingStarted>(_ => _.CompilingStarted(repository));
            // Grab all the file generators
            var fileGenerators = InvertGraphEditor.GetAllFileGenerators(config, items, true).ToArray();

            var length = 100f / (fileGenerators.Length + 1);
            var index = 0;

            foreach (var codeFileGenerator in fileGenerators)
            {
                index++;
                yield return new TaskProgress(length * index, "Generating " + System.IO.Path.GetFileName(codeFileGenerator.AssetPath));
                // Grab the information for the file
                var fileInfo = new FileInfo(codeFileGenerator.SystemPath);
                // Make sure we are allowed to generate the file
                if (!codeFileGenerator.CanGenerate(fileInfo))
                {
                    var fileGenerator = codeFileGenerator;
                    InvertApplication.SignalEvent<ICompileEvents>(_ => _.FileSkipped(fileGenerator));

                    if (codeFileGenerator.Generators.All(p => p.AlwaysRegenerate))
                    {
                        if (File.Exists(fileInfo.FullName))
                            File.Delete(fileInfo.FullName);
                    }

                    continue;
                }

                GenerateFile(fileInfo, codeFileGenerator);
                CodeFileGenerator generator = codeFileGenerator;
                InvertApplication.SignalEvent<ICompileEvents>(_ => _.FileGenerated(generator));
            }
            ChangedRecrods.Clear();
            InvertApplication.SignalEvent<ICompileEvents>(_ => _.PostCompile(config, items));
            foreach (var item in items.OfType<IGraphData>())
            {
                item.IsDirty = false;
            }
            yield return
                new TaskProgress(100f, "Complete");

#if UNITY_EDITOR
            repository.Commit();
            if (InvertGraphEditor.Platform != null) // Testability
                InvertGraphEditor.Platform.RefreshAssets();
#endif
        }

        private static void GenerateFile(FileInfo fileInfo, CodeFileGenerator codeFileGenerator)
        {
            // Get the path to the directory
            var directory = System.IO.Path.GetDirectoryName(fileInfo.FullName);
            // Create it if it doesn't exist
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            try
            {
                // Write the file
                File.WriteAllText(fileInfo.FullName, codeFileGenerator.ToString());
            }
            catch (Exception ex)
            {
                InvertApplication.LogException(ex);
                InvertApplication.Log("Coudln't create file " + fileInfo.FullName);
            }
        }


        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, params object[] obj)
        {
            var node = obj.FirstOrDefault() as DiagramNodeViewModel;
            if (node != null)
            {
                var config = InvertGraphEditor.Container.Resolve<IGraphConfiguration>();
                var fileGenerators = InvertGraphEditor.GetAllFileGenerators(config, new[] { node.DataObject as IDataRecord }, true).ToArray();
                foreach (var file in fileGenerators)
                {
                    var file1 = file;
                    if (File.Exists(file1.SystemPath))
                        ui.AddCommand(new ContextMenuItem()
                        {
                            Title = "Open " + (file.AssetPath.Replace("/", "\\")),
                            Group = "Open",
                            Command = new LambdaCommand("Open File", () =>
                            {
                                InvertGraphEditor.Platform.OpenScriptFile(file1.AssetPath);
                            })
                        });


                }

                foreach (var file in fileGenerators)
                {
                    var file1 = file;
                    var outputGen = file1.Generators.FirstOrDefault();
                    if (outputGen == null) continue;
                    var templateClassGen = outputGen as ITemplateClassGenerator;
                    if (templateClassGen != null && typeof(IOnDemandTemplate).IsAssignableFrom(templateClassGen.TemplateType))
                    {
                        ui.AddCommand(new ContextMenuItem()
                        {
                            Title = "Create Editable " + Path.GetFileName(file.AssetPath),
                            Group = "Open",
                            Command = new LambdaCommand("Create Editable File", () =>
                            {
                                GenerateFile(new FileInfo(file1.SystemPath), file1);
                                AssetDatabase.Refresh();
                                InvertGraphEditor.Platform.OpenScriptFile(file1.AssetPath);
                            })
                        });
                    }

                }
            }
        }

        public List<IDataRecord> ChangedRecrods
        {
            get { return _changedRecrods ?? (_changedRecrods = new List<IDataRecord>()); }
            set { _changedRecrods = value; }
        }

        public void RecordInserted(IDataRecord record)
        {
            if (ChangedRecrods.Contains(record)) return;
            ChangedRecrods.Add(record);
        }

        public void RecordRemoved(IDataRecord record)
        {
            ChangedRecrods.Remove(record);
        }

        public void PropertyChanged(IDataRecord record, string name, object previousValue, object nextValue)
        {
            if (ChangedRecrods.Contains(record)) return;
            ChangedRecrods.Add(record);
        }
    }
}