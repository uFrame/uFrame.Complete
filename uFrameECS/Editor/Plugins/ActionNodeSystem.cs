using System;
using System.IO;
using System.Linq;
using uFrame.ECS.Editor;
using uFrame.Editor;
using uFrame.Editor.Core;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI;
using uFrame.Editor.GraphUI.Events;
using uFrame.Editor.GraphUI.ViewModels;
using uFrame.Editor.Input;
using uFrame.Editor.Platform;
using uFrame.IOC;
using UnityEngine;

namespace uFrame.ECS.Editor
{

    public class ChangeHandlerEventCommand : Command
    {
        public HandlerNode Node;
    }

    public class CreateConverterConnectionCommand : Command
    {
        public IContextVariable Output;
        public IActionIn Input;
        public ActionMethodMetaInfo ConverterAction;

    }
    
    public class ActionNodeSystem
        : DiagramPlugin
        , IExecuteCommand<ChangeHandlerEventCommand>
        , IContextMenuQuery
        , IExecuteCommand<CreateConverterConnectionCommand>
        , IDataRecordPropertyChanged
        , IDataRecordPropertyBeforeChange


    {
        public override void Initialize(UFrameContainer container)
        {
            base.Initialize(container);
            container.RegisterInstance<IConnectionStrategy>(new ConvertConnectionStrategy(), "ConvertConnectionStrategy");
        }

        public void Execute(ChangeHandlerEventCommand command)
        {
            var selectionMenu = new SelectionMenu();
            foreach (var item in uFrameECS.Events)
            {
                var item1 = item;
                selectionMenu.AddItem(new SelectionMenuItem(item.Value, () =>
                {
                    command.Node.MetaType = item1.Value.FullName;
                }));
            }


            Signal<IShowSelectionMenu>(_ => _.ShowSelectionMenu(selectionMenu));
        }

        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, params object[] obj)
        {
            var handlerVM = obj.FirstOrDefault() as HandlerNodeViewModel;
            if (handlerVM != null)
            {
                ui.AddCommand(new ContextMenuItem()
                {
                    Title = "Change Event",
                    Group = "Events",
                    Command =
                     new ChangeHandlerEventCommand()
                     {
                         Node = handlerVM.HandlerNode
                     }
                });

            }
            var sequenceVM = obj.FirstOrDefault() as SequenceItemNodeViewModel;
            if (sequenceVM == null) return;
            var handlerNode = sequenceVM.SequenceNode.Graph.CurrentFilter as ISequenceNode;
            if (handlerNode != null)
            {
                ui.AddCommand(new ContextMenuItem()
                {
                    Title = "Set As Start Action",
                    Group = " Start",
                    Command = new LambdaCommand("Set Start", () =>
                    {
                        handlerNode.StartNode = sequenceVM.SequenceNode;
                    })
                });
            }


        }

        public void Execute(CreateConverterConnectionCommand command)
        {
            SequenceItemNode node = null;
            var type = command.ConverterAction;
            if (type != null && type.IsEditorClass)
            {
                node = Activator.CreateInstance(type.SystemType) as SequenceItemNode;
            }
            else
            {
                node = new ActionNode
                {
                    Meta = type,
                };
                //node.Name = "";
            }
            node.Graph = command.Output.Graph;

            
            InvertGraphEditor.CurrentDiagramViewModel.AddNode(node, Event.current.mousePosition - new Vector2(250f,0f));

            var result = node.GraphItems.OfType<IActionOut>().FirstOrDefault(p => p.Name == "Result");
            var input = node.GraphItems.OfType<IActionIn>().FirstOrDefault();

            node.Graph.AddConnection(command.Output, input);
            node.Graph.AddConnection(result, command.Input);

            node.IsSelected = true;
        }

        public void PropertyChanged(IDataRecord record, string name, object previousValue, object nextValue)
        {
        }

        public void BeforePropertyChanged(IDataRecord record, string name, object previousValue, object nextValue)
        {

            if (record is HandlerNode && name == "CodeHandler")
            {
                var items = record.GetCodeGeneratorsForNode(Container.Resolve<DatabaseService>().CurrentConfiguration).ToArray();
                foreach (var item in items)
                {
                    if (item.AlwaysRegenerate)
                    {
                        var fullpath = Path.Combine(Application.dataPath, item.RelativeFullPathName);
                        if (File.Exists(fullpath))
                        {
                            File.Delete(fullpath);
                        }
                    }
                }
            }
        }
    }

    public class ConvertConnectionStrategy : DefaultConnectionStrategy<IContextVariable, IActionIn>
    {
        public override Color ConnectionColor
        {
            get { return Color.white; }
        }

        public override void Remove(ConnectorViewModel output, ConnectorViewModel input)
        {

        }

        protected override bool CanConnect(IContextVariable output, IActionIn input)
        {
            foreach (var p in uFrameECS.Converters)
            {
                if (output.VariableType.IsAssignableTo(p.ActionFields.First(x => !x.IsReturn).MemberType))
                {
                    if (p.ActionFields.First(x => x.IsReturn).MemberType.FullName == input.VariableType.FullName)
                        return true;
                }
                
                   
            }
            return false;

            return true;
        }

        //public override ConnectionViewModel Connect(DiagramViewModel diagramViewModel, ConnectorViewModel a, ConnectorViewModel b)
        //{
        //    //var converter = uFrameECS.Converters.FirstOrDefault(p =>
        //    //  p.ActionFields.First(x => !x.IsReturn).MemberType.IsAssignableTo(output.VariableType) &&
        //    //  p.ActionFields.First(x => x.IsReturn).MemberType.FullName == input.GetType().FullName
        //    //  );
        //    foreach (var p in uFrameECS.Converters)
        //    {
        //        //if (p.ActionFields.First(x => !x.IsReturn).MemberType.IsAssignableTo(output.VariableType))
        //        //{
        //        //    if (p.ActionFields.First(x => x.IsReturn).MemberType.FullName == input.GetType().FullName)
        //        //    {
        //        //        CreateConnection(diagramViewModel, a, b, Apply);
        //        //    }
        //        //}


        //    }
        //}
        public override void Apply(ConnectionViewModel connectionViewModel)
        {
            base.Apply(connectionViewModel);
        }

        protected override void ApplyConnection(IGraphData graph, IConnectable output, IConnectable input)
        {
            //base.ApplyConnection(graph, output, input);
            ApplyConnection(graph, output as IContextVariable, input as IActionIn);
        }

        protected override void ApplyConnection(IGraphData graph, IContextVariable output, IActionIn input)
        {
            base.ApplyConnection(graph, output, input);
           
            var converter = uFrameECS.Converters.FirstOrDefault(p =>
               output.VariableType.IsAssignableTo(p.ConvertFrom.MemberType) &&
               p.ConvertTo.MemberType.FullName == input.VariableType.FullName
               );
            if (converter != null)
            {
                InvertApplication.Execute(new CreateConverterConnectionCommand()
                {
                    ConverterAction = converter,
                    Input = input,
                    Output = output
                });
            }
        }
    }
    public class PickupCommand : Command
    {

    }

    public class DropCommand : Command
    {

    }


    public class CopyCommand : Command
    {
        
    }

    public class PasteCommand : Command
    {
        public Vector2 Position { get; set; }
    }
}