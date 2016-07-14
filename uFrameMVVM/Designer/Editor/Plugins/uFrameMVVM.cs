using uFrame.Editor.Compiling.CommonNodes;
using uFrame.Editor.Workspaces;
using uFrame.IOC;
using System;
using System.CodeDom;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using uFrame.Editor;
using uFrame.Editor.Configurations;
using uFrame.Editor.Core;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;
using uFrame.MVVM.Templates;
using uFrame.Editor.GraphUI;
using uFrame.Editor.GraphUI.Events;
using uFrame.Editor.GraphUI.ViewModels;
using uFrame.Editor.Menus;
using uFrame.Editor.Platform;
using uFrame.Editor.TypesSystem;
using uFrame.Kernel;
using uFrame.MVVM.Services;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = System.Object;

namespace uFrame.MVVM
{
    public class uFrameMVVM : uFrameMVVMBase
        , IToolbarQuery
        , IExecuteCommand<ScaffoldOrUpdateKernelCommand>
        , IQueryTypes
        , IQueryPossibleConnections
    {
        static uFrameMVVM()
        {
            InvertApplication.CachedTypeAssembly(typeof(uFrameMVVM).Assembly);
            InvertApplication.TypeAssemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(p => p.FullName.StartsWith("Assembly")));
        }

        public override decimal LoadPriority
        {
            get { return 500; }
        }

        public override void Initialize(UFrameContainer container)
        {
            base.Initialize(container);
            SubSystem.Name = "SubSystem";
            SceneType.Name = "Scene Type";
            container.AddWorkspaceConfig<MvvmWorkspace>("MVVM")
                .WithGraph<MVVMGraph>("MVVM", "Create MVVM")
                .WithGraph<SubSystemGraph>("SubSystem", "Create SubSystem");
            MVVM.HasSubNode<TypeReferenceNode>();
            SubSystem.HasSubNode<TypeReferenceNode>();
            SubSystem.HasSubNode<EnumNode>();
            Service.HasSubNode<TypeReferenceNode>();
            Service.HasSubNode<EnumNode>();
            Element.HasSubNode<TypeReferenceNode>();
            Element.HasSubNode<EnumNode>();

            uFrameMVVM.BindingTypes = InvertGraphEditor.Container.Instances.Where(p => p.Key.Item1 == typeof(uFrameBindingType)).ToArray();
        }

        public static KeyValuePair<Tuple<Type, string>, object>[] BindingTypes { get; set; }

        public void QueryToolbarCommands(ToolbarUI ui)
        {
            ui.AddCommand(new ToolbarItem()
            {
                Command = new ScaffoldOrUpdateKernelCommand(),
                Title = "Update Kernel",
                Position = ToolbarPosition.Right,
                Description = "Start Create/Update MVVM Kernel.",
                Order = -1,
                IsDelayCall = true
            });
        }

        public void Execute(ScaffoldOrUpdateKernelCommand command)
        {
            var firstOrDefault = InvertApplication.Container.Resolve<WorkspaceService>()
                                                            .CurrentWorkspace
                                                            .Graphs
                                                            .FirstOrDefault(g => g is MVVMGraph);
            if (firstOrDefault == null) return;
            var mvvmNode = firstOrDefault.NodeItems.FirstOrDefault(n => n is MVVMNode);
            if(mvvmNode == null) return;
            command.Perform(mvvmNode as MVVMNode);
        }

        public void QueryTypes(List<ITypeInfo> typeInfo)
        {
            GraphTypeInfo[] typeinfos = InvertGraphEditor.TypesContainer.ResolveAll<GraphTypeInfo>().ToArray();
            foreach (var item in typeinfos)
            {
                typeInfo.Add(new SystemTypeInfo(item.Type));
            }
        }

        public void QueryPossibleConnections(SelectionMenu menu, DiagramViewModel diagramViewModel, ConnectorViewModel startConnector, Vector2 mousePosition)
        {
            if (startConnector.ConnectorForType.FullName == typeof(ElementNode).FullName)
            {
                menu.Items.Clear();
                var vm = InvertGraphEditor.CurrentDiagramViewModel;

                var category = new SelectionMenuCategory()
                {
                    Title = "Connect"
                };

                menu.AddItem(category);

                menu.AddItem(new SelectionMenuItem("Connect", "Create View Node and Connect to : Element", () =>
                {
                    ViewNode viewNode = new ViewNode();
                    vm.AddNode(viewNode, vm.LastMouseEvent.LastMousePosition);
                    diagramViewModel.GraphData.AddConnection(startConnector.ConnectorFor.DataObject as IConnectable, viewNode.ElementInputSlot);
                }), category);
            }
        }
    }

    public class ScaffoldOrUpdateKernelCommand : Command
    {
        public void Perform(MVVMNode node)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            if (!EditorUtility.DisplayDialog("Warning!", "Before scaffolding the core, make sure you saved and compiled!", 
                                             "Yes, I saved and compiled!", 
                                             "Cancel")) return;

            var paths = InvertApplication.Container.Resolve<DatabaseService>().CurrentConfiguration.CodeOutputPath + "/";
            var scenesPath = System.IO.Path.Combine(paths, "Scenes");

            var sceneName = node.Graph.Namespace + "KernelScene.unity";
            var sceneNameWithPath = System.IO.Path.Combine(scenesPath, sceneName);

            var prefabName = node.Graph.Namespace + "Kernel.prefab";
            var prefabNameWithPath = Path2.Combine(paths, prefabName);
            var relativeScenesPath = System.IO.Path.Combine(paths, "Scenes");
            var relativeScenePath = System.IO.Path.Combine(relativeScenesPath + "/", sceneName);
            uFrameKernel uFrameMVVMKernel = null;

            if (File.Exists(sceneNameWithPath))
            {
                var gameObject = (GameObject)AssetDatabase.LoadAssetAtPath(prefabNameWithPath, typeof(GameObject));
                uFrameMVVMKernel = gameObject.GetComponent<uFrameKernel>();
                SyncKernel(node, uFrameMVVMKernel.gameObject);
            }
            else
            {
                var eScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
                if (!Directory.Exists(scenesPath))
                {
                    Directory.CreateDirectory(scenesPath);
                }
                uFrameMVVMKernel = FindComponentInScene<uFrameKernel>() ?? new GameObject("Kernel").AddComponent<uFrameKernel>();
                var services = SyncKernel(node, uFrameMVVMKernel.gameObject);

                services.gameObject.AddComponent<ViewService>();
                services.gameObject.AddComponent<SceneManagementService>();

                PrefabUtility.CreatePrefab(prefabNameWithPath, uFrameMVVMKernel.gameObject, ReplacePrefabOptions.ConnectToPrefab);
                EditorSceneManager.SaveScene(eScene, relativeScenePath);
            }

            if (!UnityEditor.EditorBuildSettings.scenes.Any(s => s.path.EndsWith(node.Graph.SystemPath + "KernelScene.unity")))
            {
                var list = EditorBuildSettings.scenes.ToList();
                list.Add(new EditorBuildSettingsScene(relativeScenePath, true));
                EditorBuildSettings.scenes = list.ToArray();
            }
            AssetDatabase.Refresh();
        }

        private static Transform SyncKernel(MVVMNode node, GameObject uFrameMVVMKernel)
        {
            var servicesContainer = uFrameMVVMKernel.transform.FindChild("Services");
            if (servicesContainer == null)
            {
                servicesContainer = new GameObject("Services").transform;
                servicesContainer.SetParent(uFrameMVVMKernel.transform);
            }

            var systemLoadersContainer = uFrameMVVMKernel.transform.FindChild("SystemLoaders");
            if (systemLoadersContainer == null)
            {
                systemLoadersContainer = new GameObject("SystemLoaders").transform;
                systemLoadersContainer.SetParent(uFrameMVVMKernel.transform);
            }

            var sceneLoaderContainer = uFrameMVVMKernel.transform.FindChild("SceneLoaders");
            if (sceneLoaderContainer == null)
            {
                sceneLoaderContainer = new GameObject("SceneLoaders").transform;
                sceneLoaderContainer.SetParent(uFrameMVVMKernel.transform);
            }

            //var servicesNodes = InvertApplication.Container.Resolve<WorkspaceService>().CurrentWorkspace.Graphs
            //                                               .SelectMany(g => g.AllGraphItems.OfType<ServiceNode>());
            var servicesNodes = InvertApplication.Container.Resolve<WorkspaceService>()
                                                 .Workspaces.SelectMany(w => w.Graphs)
                                                 .SelectMany(g => g.AllGraphItems.OfType<ServiceNode>());
            foreach (var serviceNode in servicesNodes)
            {
                //var type = InvertApplication.FindType(serviceNode.FullName);
                var type = InvertApplication.FindRuntimeType(serviceNode.FullName);
                if (type != null && servicesContainer.GetComponent(type) == null)
                {
                    servicesContainer.gameObject.AddComponent(type);
                }
            }

            //var systemNodes = InvertApplication.Container.Resolve<WorkspaceService>().CurrentWorkspace.Graphs
            //                                   .SelectMany(g => g.AllGraphItems.OfType<SubSystemNode>());
            var systemNodes = InvertApplication.Container.Resolve<WorkspaceService>()
                                                         .Workspaces.SelectMany(w => w.Graphs)
                                                         .SelectMany(g => g.AllGraphItems.OfType<SubSystemNode>());
            foreach (var systemNode in systemNodes)
            {
                //var type = InvertApplication.FindType(string.Format("{0}Loader", systemNode.FullName));
                var type = InvertApplication.FindRuntimeType(string.Format("{0}Loader", systemNode.FullName));
                if (type != null && systemLoadersContainer.GetComponent(type) == null)
                {
                    systemLoadersContainer.gameObject.AddComponent(type);
                }
            }

            //var sceneNodes = node.Graph.AllGraphItems.OfType<SceneTypeNode>();
            var sceneNodes = InvertApplication.Container.Resolve<WorkspaceService>()
                                                        .Workspaces.SelectMany(w => w.Graphs)
                                                        .SelectMany(g => g.AllGraphItems.OfType<SceneTypeNode>());
            foreach (var sceneNode in sceneNodes)
            {
                //var type = InvertApplication.FindType(string.Format("{0}Loader", sceneNode.FullName));
                var type = InvertApplication.FindRuntimeType(string.Format("{0}Loader", sceneNode.FullName));
                if (type != null && sceneLoaderContainer.GetComponent(type) == null)
                {
                    sceneLoaderContainer.gameObject.AddComponent(type);
                }
            }


            EditorUtility.SetDirty(uFrameMVVMKernel);
            return servicesContainer;
        }


        private T FindComponentInScene<T>() where T : MonoBehaviour
        {
            object[] obj = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
            foreach (object o in obj)
            {
                GameObject g = (GameObject)o;
                var c = (T)g.GetComponent(typeof(T));
                if (c != null) return c;
            }
            return null;
        }
    }
}
