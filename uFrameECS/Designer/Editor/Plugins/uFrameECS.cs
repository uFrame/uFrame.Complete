using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using uFrame.Attributes;
using uFrame.Editor;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Compiling.CommonNodes;
using uFrame.Editor.Configurations;
using uFrame.Editor.Core;
using uFrame.Editor.Database;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Documentation;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;
using uFrame.Editor.GraphUI;
using uFrame.Editor.GraphUI.Drawers;
using uFrame.Editor.GraphUI.Events;
using uFrame.Editor.GraphUI.ViewModels;
using uFrame.Editor.Input;
using uFrame.Editor.NavigationSystem;
using uFrame.Editor.Platform;
using uFrame.Editor.QuickAccess;
using uFrame.Editor.TypesSystem;
using uFrame.Editor.WindowsPlugin;
using uFrame.Editor.Workspaces;
using uFrame.Editor.Workspaces.Commands;
using uFrame.IOC;
using UnityEngine;
using UnityEngine.UI;

namespace uFrame.ECS.Editor
{
    public class uFrameECS : uFrameECSBase,
        IContextMenuQuery, IQuickAccessEvents, IOnMouseDoubleClickEvent,
        IExecuteCommand<AddSlotInputNodeCommand>,
        IExecuteCommand<NewModuleWorkspace>,
        IQueryPossibleConnections,
        IExecuteCommand<GroupActionNodes>,
        IQueryTypes,
        IDocumentationProvider,
        IUpgradeDatabase,
        ICompilingStarted, IGraphSelectionEvents
    {
        #region Properties
        public override decimal LoadPriority
        {
            get { return 500; }
        }

        public static Type EcsComponentType
        {
            get;
            set;
        }
        public static Type EcsGroupType
        {
            get;
            set;
        }
        public static Type EntityComponentType
        {
            get;
            set;
        }

        public IEnumerable<Type> EventTypes
        {
            get
            {
                foreach (var assembly in InvertApplication.TypeAssemblies)
                {
                    foreach (var type in assembly.GetTypes())
                    {

                        if (type.IsDefined(typeof(uFrameEvent), true))
                        {
                            yield return type;
                        }
                    }
                }
            }
        }

        private IHandlerCodeWriter[] _codeWriters;
        public IHandlerCodeWriter[] CodeWriters
        {
            get
            {
                return _codeWriters ??
                       (_codeWriters = EventCodeWriterTypes.Select(p => Activator.CreateInstance(p)).Cast<IHandlerCodeWriter>().ToArray());
            }
        }

        private static ActionMethodMetaInfo[] _converters;

        public static ActionMethodMetaInfo[] Converters
        {
            get
            {
                return _converters ??
                       (_converters =
                           uFrameECS.Actions.Values.OfType<ActionMethodMetaInfo>().Where(p => p.IsConverter).ToArray());
            }
        }

        public IEnumerable<Type> EventCodeWriterTypes
        {
            get
            {
                foreach (var assembly in InvertApplication.CachedAssemblies)
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.IsClass && !type.IsAbstract && typeof(IHandlerCodeWriter).IsAssignableFrom(type))
                        {
                            yield return type;
                        }
                    }
                }
            }
        }

        public MouseEvent LastMouseEvent { get; set; }

        public static HashSet<Type> SystemTypes
        {
            get { return _types; }
        }

        private static Dictionary<string, IActionMetaInfo> _actions;

        private static Dictionary<string, EventMetaInfo> _events;

        private readonly static HashSet<Type> _types = new HashSet<Type>();

        #endregion

        static uFrameECS()
        {
            InvertApplication.CachedAssembly(typeof(Button).Assembly);
            InvertApplication.CachedTypeAssembly(typeof(uFrameECS).Assembly);
            InvertApplication.TypeAssemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(p => p.FullName.StartsWith("Assembly")));
        }

        public override void Initialize(UFrameContainer container)
        {
            base.Initialize(container);
            container.RegisterInstance<IDocumentationProvider>(this, "ECS");
            container.RegisterInstance<IExplorerProvider>(new EventsExplorerProvider(), "Events");
            container.RegisterConnectionStrategy<ECSConnectionStrategy>();
            container.RegisterGraphItem<HandlerNode, HandlerNodeViewModel, HandlerNodeDrawer>();
            container.RegisterGraphItem<CustomAction, CustomActionViewModel, SequenceItemNodeDrawer>();
            container.RegisterDrawer<ItemViewModel<IContextVariable>, ItemDrawer>();
            container.AddWorkspaceConfig<EcsWorkspace>("ECS")
                .WithGraph<LibraryGraph>("Library", "Create components, groups, events, custom actions, and more.")
                .WithGraph<SystemGraph>("System", "System defines behaviour for items defines inside a your libraries.")
                .WithGraph<ModuleGraph>("Module", "A module graph allows you to comprise library items and system as a single graph.")
                ;
            //container.AddWorkspaceConfig<BehaviourWorkspace>("Behaviour").WithGraph<SystemGraph>("System Graph");
            //container.AddWorkspaceConfig<LibraryWorkspace>("Library").WithGraph<LibraryGraph>("Library Graph");
            InvertGraphEditor.TypesContainer.RegisterInstance(new GraphTypeInfo()
            {
                Type = typeof(IDisposable),
                IsPrimitive = false,
                Label = "Disposable",
                Name = "Disposable"
            }, "IDisposable");
            //container.RegisterDrawer<NoteNodeViewModel, NoteNodeDrawer>();
            //container.AddItemFlag<ComponentsReference>("Multiple", UnityEngine.Color.blue);
            //container.AddItemFlag<PropertiesChildItem>("Mapping", UnityEngine.Color.blue);
            //container.AddItemFlag<PropertiesChildItem>("HideInUnityInspector", CachedStyles.GetColor(NodeColor.Azure4));
            //container.AddNodeFlag<EventNode>("Dispatcher");
            //System.HasSubNode<EnumNode>();

            CollectionItemAdded.Name = "Collection Item Added Handler";
            CollectionItemRemoved.Name = "Collection Item Removed Handler";
            PropertyChanged.Name = "Property Changed Handler";
            CustomAction.Name = "Custom Action";
            System.Name = "System";
            Handler.Name = "Handler";
            ComponentCreated.Name = "Component Created Handler";
            ComponentDestroyed.Name = "Component Destroyed Handler";
            EnumValue.Name = "Enum Value";
            //VariableReference.Name = "Var";

            Handler.AllowAddingInMenu = false;
            UserMethod.AllowAddingInMenu = false;
            Action.AllowAddingInMenu = false;
            SequenceItem.AllowAddingInMenu = false;
            //ComponentGroup.AllowAddingInMenu = false;
            //VariableReference.AllowAddingInMenu = false;

            Action.NodeColor.Literal = NodeColor.Green;

            Group.HasSubNode<EnumValueNode>();
            Library.HasSubNode<EnumNode>();
            Library.HasSubNode<TypeReferenceNode>();
            Module.HasSubNode<TypeReferenceNode>();
            Module.HasSubNode<NoteNode>();
            Module.HasSubNode<EnumNode>();
            Module.HasSubNode<ComponentNode>();
            System.HasSubNode<EnumNode>();
            System.HasSubNode<TypeReferenceNode>();
            //System.HasSubNode<ComponentNode>();
            //System.HasSubNode<TypeReferenceNode>();
            //System.HasSubNode<ComponentNode>();
            //System.HasSubNode<ContextNode>(); 

            container.Connectable<IContextVariable, IActionIn>();
            container.Connectable<IActionOut, IContextVariable>();
            container.Connectable<ActionBranch, SequenceItemNode>();
            container.Connectable<IMappingsConnectable, HandlerIn>();
            container.Connectable<ActionBranch, BranchesChildItem>();
            container.Connectable<IContextVariable, OutputsChildItem>();
            container.Connectable<SequenceItemNode, BranchesChildItem>();

            SystemTypes.Add(typeof(Button));
            SystemTypes.Add(typeof(UnityEngine.UI.LayoutElement));
            SystemTypes.Add(typeof(UnityEngine.UI.LayoutGroup));
            SystemTypes.Add(typeof(UnityEngine.UI.GridLayoutGroup));
            SystemTypes.Add(typeof(UnityEngine.UI.Text));
            SystemTypes.Add(typeof(UnityEngine.UI.InputField));
            SystemTypes.Add(typeof(UnityEngine.UI.ScrollRect));
            SystemTypes.Add(typeof(UnityEngine.UI.Scrollbar));
            SystemTypes.Add(typeof(UnityEngine.UI.Outline));
            SystemTypes.Add(typeof(UnityEngine.UI.Toggle));
            SystemTypes.Add(typeof(UnityEngine.UI.ToggleGroup));
            SystemTypes.Add(typeof(UnityEngine.UI.Slider));
            SystemTypes.Add(typeof(UnityEngine.Transform));
            SystemTypes.Add(typeof(UnityEngine.PlayerPrefs));
            SystemTypes.Add(typeof(UnityEngine.Application));
            //SystemTypes.Add(typeof (UnityEngine.UI.Dropdown));

            AddHandlerType(typeof(PropertyChangedNode));
            AddHandlerType(typeof(ComponentDestroyedNode));
            AddHandlerType(typeof(ComponentCreatedNode));
            AddHandlerType(typeof(ActionGroupNode));
            AddHandlerType(typeof(CollectionItemAddedNode));
            AddHandlerType(typeof(CollectionItemRemovedNode));
            AddHandlerType(typeof(CustomActionNode));
        }

        private static void AddHandlerType(Type type)
        {
            var propertyTypes = FilterExtensions.AllowedFilterNodes[type] = new List<Type>();
            foreach (var item in FilterExtensions.AllowedFilterNodes[typeof(HandlerNode)])
            {
                propertyTypes.Add(item);
            }
        }

        public override void Loaded(UFrameContainer container)
        {
            base.Loaded(container);
            LoadActions();
            LoadEvents();
        }

        #region Load Actions
        public void LoadActions()
        {
            var actions = new List<IActionMetaInfo>();
            Signal<IQueryActionMetaInfo>(_ => _.QueryActions(actions));
            Actions.Clear();
            foreach (var item in actions)
            {
                if (!Actions.ContainsKey(item.Identifier))
                {

                    Actions.Add(item.Identifier, item);

                    // BAckwards compatability ERGH!!
                    if (item.FullName != item.Identifier)
                    {
                        if (!Actions.ContainsKey(item.FullName))
                            Actions.Add(item.FullName, item);
                    }
                }
            }
        }

        public static Dictionary<string, IActionMetaInfo> Actions
        {
            get { return _actions ?? (_actions = new Dictionary<string, IActionMetaInfo>()); }
            set { _actions = value; }
        }
        #endregion

        #region Load Events
        private void LoadEvents()
        {

            Events.Clear();
            foreach (var eventType in EventTypes)
            {
                if (Events.ContainsKey(eventType.FullName)) continue;
                var eventInfo = new EventMetaInfo(eventType)
                {

                };

                eventInfo.Attribute =
                    eventType.GetCustomAttributes(typeof(uFrameEvent), true).OfType<uFrameEvent>().FirstOrDefault();

                //var fields = eventType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                //var properties = eventType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);


                //foreach (var field in fields)
                //{
                //    var fieldMetaInfo = new EventFieldInfo()
                //    {
                //        Type = field.FieldType,
                //        Attribute = eventType.GetCustomAttributes(typeof(uFrameEventMapping), true).OfType<uFrameEventMapping>().FirstOrDefault(),
                //        Name = field.Name
                //    };
                //    if (!SystemTypes.Contains(field.FieldType))
                //        SystemTypes.Add(field.FieldType);

                //    eventInfo.Members.Add(fieldMetaInfo);
                //}
                //foreach (var field in properties)
                //{
                //    var fieldMetaInfo = new EventFieldInfo()
                //    {
                //        Type = field.PropertyType,
                //        Name = field.Name,
                //        Attribute = eventType.GetCustomAttributes(typeof(uFrameEventMapping), true).OfType<uFrameEventMapping>().FirstOrDefault(),
                //        IsProperty = true
                //    };

                //    if (!SystemTypes.Contains(field.PropertyType))
                //        SystemTypes.Add(field.PropertyType);

                //    eventInfo.Members.Add(fieldMetaInfo);
                //}
                Events.Add(eventType.FullName, eventInfo);
            }
        }

        public static Dictionary<string, EventMetaInfo> Events
        {
            get { return _events ?? (_events = new Dictionary<string, EventMetaInfo>()); }
            set { _events = value; }
        }
        #endregion

        #region Query Methods

        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, params object[] objs)
        {
            var obj = objs.FirstOrDefault();
            if (obj is InputOutputViewModel)
            {
                QuerySlotMenu(ui, (InputOutputViewModel)obj);
            }
            var handlerVM = obj as HandlerNodeViewModel;
            if (handlerVM != null)
            {
                var handler = handlerVM.Handler;
                ui.AddCommand(new ContextMenuItem()
                {
                    Title = "Code Handler (Obsolete)",
                    Checked = handler.CodeHandler,
                    Command = new LambdaCommand(
                        "Toggle Code Handler",
                        () =>
                        {
                            handler.CodeHandler = !handler.CodeHandler;
                        })
                });
                ui.AddCommand(new ContextMenuItem()
                {
                    Title = "Custom (Obsolete)",
                    Checked = handler.Custom,
                    Command = new LambdaCommand(
                    "Toggle Custom Handler",
                () =>
                {

                    handler.Custom = !handler.Custom;
                })
                });
                foreach (var handlerIn in handler.HandlerInputs)
                {
                    if (handlerIn.Item != null)
                    {
                        ui.AddCommand(new ContextMenuItem()
                        {
                            Title = "Navigate To " + handlerIn.Item.Name,
                            Group = "Navigation",
                            Command = new NavigateToNodeCommand()
                            {
                                Node = handlerIn.Item as IDiagramNode
                            }
                        });
                    }
                }
            }

            var seqVM = obj as SequenceItemNodeViewModel;

            if (seqVM != null)
            {
                var node = seqVM.SequenceNode as PublishEventNode;
                if (node != null)
                {
                    var evtNode = node.SelectedEvent as EventNode;
                    if (evtNode != null)
                    {
                        ui.AddCommand(new ContextMenuItem()
                        {
                            Title = "Go To Definition",
                            Group = "Navigation",
                            Command = new NavigateToNodeCommand()
                            {
                                Node = evtNode
                            }
                        });
                    }
                }
            }

            //var nodeViewModel = obj as SequenceItemNodeViewModel;
            //if (nodeViewModel != null)
            //{
            //    var node = nodeViewModel.SequenceNode;
            //    ui.AddCommand(new ContextMenuItem()
            //    {
            //        Title = "Move To Group",
            //        Command = new GroupActionNodes()
            //        {
            //            Node = node
            //        }
            //    });
            //}

            var diagramViewModel = obj as DiagramViewModel;

            if (diagramViewModel != null)
            {
                var contextVar = diagramViewModel.GraphData.CurrentFilter as IVariableContextProvider;
                if (contextVar != null)
                {


                    foreach (var item in contextVar.GetAllContextVariables())
                    {
                        var item1 = item;
                        foreach (var child in item.GetPropertyDescriptions())
                        {
                            var child1 = child;
                            ui.AddCommand(new ContextMenuItem()
                            {
                                Title = item1.ShortName + "/" + child1.ShortName,
                                Group = "Variables",
                                Command = new LambdaCommand("Add Variable",
                                    () =>
                                    {
                                        var node = new PropertyNode()
                                        {
                                            Graph = diagramViewModel.GraphData,
                                        };
                                        diagramViewModel.AddNode(node, evt.LastMousePosition).Collapsed = true;
                                        node.Object.SetInput(item1);
                                        node.PropertySelection.SetInput(child1);
                                        node.IsSelected = true;
                                    })
                            });
                        }
                    }

                }
            }


        }

        private void QuerySlotMenu(ContextMenuUI ui, InputOutputViewModel slot)
        {

            //ui.AddCommand(new ContextMenuItem()
            //{
            //    Title = "Test",
            //    Command = new LambdaCommand("test", () =>
            //    {

            //        var variableIn = slot.DataObject as VariableIn;
            //        foreach (var item in variableIn.Inputs)
            //        {
            //            InvertApplication.Log(item.Input.Title);
            //            InvertApplication.Log(item.Output.Title);
            //        }
            //    })
            //});

        }

        public void QuickAccessItemsEvents(QuickAccessContext context, List<IItem> items)
        {

            //            if (context.ContextType == typeof (IInsertQuickAccessContext))
            //            {
            //                items.Clear();
            //                items.AddRange(QueryInsert(context));
            //            }
            //            if (context.ContextType == typeof (IConnectionQuickAccessContext))
            //            {
            //                if (InvertApplication.Container.Resolve<WorkspaceService>().CurrentWorkspace.CurrentGraph.CurrentFilter is HandlerNode)
            //                {
            //             
            //                    items.Clear();
            //                    items.AddRange(QueryConntectionActions(context));
            //                }
            //                
            //            }
        }

        private void QueryInsert(SelectionMenu menu)
        {
            var mousePosition = UnityEngine.Event.current.mousePosition;
            var currentGraph = InvertApplication.Container.Resolve<WorkspaceService>().CurrentWorkspace.CurrentGraph;
            var systemNode = currentGraph.CurrentFilter as SystemNode;
            if (systemNode != null)
            {
                var category = new SelectionMenuCategory()
                {
                    Title = "Events",
                    Expanded = true,
                    Description = "This category includes events exposed by ECS as well as any custom events."
                };

                menu.AddItem(category);
                foreach (var item in currentGraph.Repository.All<EventNode>())
                {
                    var item1 = item;
                    var qa = new SelectionMenuItem(item, () =>
                    {

                        var eventNode = new HandlerNode()
                        {
                            MetaType = item1.Identifier,
                            Name = systemNode.Name + item1.Name
                        };
                        InvertGraphEditor.CurrentDiagramViewModel.AddNode(eventNode, LastMouseEvent != null ? LastMouseEvent.MousePosition : new Vector2(0, 0));
                    }) { Group = "Handlers" };
                    menu.AddItem(qa, category);
                }
                foreach (var item in Events)
                {
                    var item1 = item;
                    var qa = new SelectionMenuItem(item.Value, () =>
                    {
                        var eventNode = new HandlerNode()
                        {
                            Meta = item1.Value,
                            Name = systemNode.Name + item1.Value.Title
                        };
                        InvertGraphEditor.CurrentDiagramViewModel.AddNode(eventNode, LastMouseEvent != null ? LastMouseEvent.MousePosition : new Vector2(0, 0));
                    });
                    menu.AddItem(qa, category);
                }
            }
            if (currentGraph.CurrentFilter is SequenceItemNode)
            {
                var vm = InvertGraphEditor.CurrentDiagramViewModel;

                var category = new SelectionMenuCategory()
                {
                    Title = "ECS Variables"
                };

                menu.AddItem(category);

                menu.AddItem(new SelectionMenuItem("Set", "Set Variable", () => { vm.AddNode(new SetVariableNode(), vm.LastMouseEvent.LastMousePosition); }), category);

                menu.AddItem(new SelectionMenuItem("Create", "Bool Variable", () =>
                {
                    Execute(new CreateNodeCommand() { GraphData = vm.GraphData, Position = vm.LastMouseEvent.MouseDownPosition, NodeType = typeof(BoolNode) });

                }), category);
                menu.AddItem(new SelectionMenuItem("Create", "Vector2 Variable", () => { vm.AddNode(new Vector2Node(), vm.LastMouseEvent.LastMousePosition); }), category);
                menu.AddItem(new SelectionMenuItem("Create", "Vector3 Variable", () => { vm.AddNode(new Vector3Node(), vm.LastMouseEvent.LastMousePosition); }), category);
                menu.AddItem(new SelectionMenuItem("Create", "String Variable", () => { vm.AddNode(new StringNode(), vm.LastMouseEvent.LastMousePosition); }), category);
                menu.AddItem(new SelectionMenuItem("Create", "Float Variable", () => { vm.AddNode(new FloatNode(), vm.LastMouseEvent.LastMousePosition); }), category);
                menu.AddItem(new SelectionMenuItem("Create", "Integer Variable", () => { vm.AddNode(new IntNode(), vm.LastMouseEvent.LastMousePosition); }), category);
                menu.AddItem(new SelectionMenuItem("Create", "Literal", () => { vm.AddNode(new LiteralNode(), vm.LastMouseEvent.LastMousePosition); }), category);

                //var currentFilter = currentGraph.CurrentFilter as HandlerNode;
                //foreach (var item in currentFilter.GetAllContextVariables())
                //{
                //    var item1 = item;
                //    var qa = new QuickAccessItem("Variables", item.VariableName ?? "Unknown", _ =>
                //    {
                //        var command = new AddVariableReferenceCommand()
                //        {
                //            Variable = _ as IContextVariable,
                //            Handler = currentFilter,
                //            Position = mousePosition
                //        };
                //        // TODO 2.0 Add Variable Reference COmmand
                //        //InvertGraphEditor.ExecuteCommand(command);
                //    })
                //    {
                //        Item = item1
                //    };
                //    yield return qa;
                //}
                QueryActions(menu);
            }


        }

        private void QueryActions(SelectionMenu menu)
        {
            var mousePosition = UnityEngine.Event.current.mousePosition;
            var diagramViewModel = InvertGraphEditor.CurrentDiagramViewModel;
            GetActionsMenu(menu, _ =>
            {
                SequenceItemNode node = null;
                var type = _ as ActionMetaInfo;
                if (type != null && type.IsEditorClass)
                {
                    node = Activator.CreateInstance(type.SystemType) as SequenceItemNode;
                }
                else
                {
                    node = new ActionNode
                    {
                        Meta = _,
                    };
                    //node.Name = "";
                }
                node.Graph = diagramViewModel.GraphData;
                diagramViewModel.AddNode(node, mousePosition);
                node.IsSelected = true;

            });
        }

        public void QueryPossibleConnections(SelectionMenu menu, DiagramViewModel diagramViewModel, ConnectorViewModel startConnector, Vector2 mousePosition)
        {
            var contextVar = startConnector.ConnectorFor.DataObject as IContextVariable;
            if (contextVar != null)
            {
                menu.Items.Clear();
                foreach (var item in contextVar.GetPropertyDescriptions())
                {
                    var item1 = item;
                    menu.AddItem(new SelectionMenuItem(contextVar.ShortName, item.ShortName, () =>
                    {
                        var node = new PropertyNode()
                        {
                            Graph = diagramViewModel.GraphData,
                        };
                        diagramViewModel.AddNode(node, mousePosition).Collapsed = true;
                        diagramViewModel.GraphData.AddConnection(startConnector.ConnectorFor.DataObject as IConnectable, node.Object);
                        node.PropertySelection.SetInput(item1);
                        node.IsSelected = true;
                    }));
                }
                //foreach (var item in contextVar.VariableType.GetMembers().OfType<IMethodMemberInfo>())
                //{
                //    var item1 = item;
                //    menu.AddItem(new SelectionMenuItem(contextVar.ShortName, item.MethodIdentifier, () =>
                //    {

                //    }));
                //}

            }

            if (startConnector.ConnectorFor.DataObject is IVariableContextProvider)
            {
                menu.Items.Clear();
                GetActionsMenu(menu, _ =>
                {
                    SequenceItemNode node = null;
                    var type = _ as ActionMetaInfo;
                    if (type != null && type.IsEditorClass)
                    {
                        node = Activator.CreateInstance(type.SystemType) as SequenceItemNode;
                    }
                    else
                    {

                        node = new ActionNode
                        {
                            Meta = _,
                        };
                        //node.Name = "";
                    }
                    node.Graph = diagramViewModel.GraphData;
                    diagramViewModel.AddNode(node, mousePosition);
                    diagramViewModel.GraphData.AddConnection(startConnector.ConnectorFor.DataObject as IConnectable, node);
                    node.IsSelected = true;
                    node.Name = "";
                });
            }

        }

        public void QueryTypes(List<ITypeInfo> typeInfo)
        {
            foreach (var item in SystemTypes)
            {
                typeInfo.Add(new SystemTypeInfo(item));
            }
            foreach (var item in Events)
            {
                typeInfo.Add(item.Value);
            }
        }

        private IEnumerable<IItem> QueryConntectionActions(QuickAccessContext context)
        {
            var connectionHandler = context.Data as ConnectionHandler;
            var diagramViewModel = connectionHandler.DiagramViewModel;

            var category = new QuickAccessCategory()
            {
                Title = "Connections"
            };

            foreach (var item in Actions)
            {

                var qaItem = new QuickAccessItem(item.Value.CategoryPath.FirstOrDefault() ?? string.Empty, item.Value.Title, item.Value.Title, _ =>
                {
                    var actionInfo = _ as ActionMetaInfo;
                    var node = new ActionNode()
                    {
                        Meta = actionInfo
                    };
                    node.Graph = diagramViewModel.GraphData;


                    diagramViewModel.AddNode(node, context.MouseData.MouseUpPosition);
                    diagramViewModel.GraphData.AddConnection(connectionHandler.StartConnector.ConnectorFor.DataObject as IConnectable, node);
                    node.IsSelected = true;
                    node.Name = "";
                })
                {
                    Item = item.Value
                };
                category.Add(qaItem);
            }
            yield return category;
        }
        #endregion 

        #region Get Methods
        private void GetActionsMenu(SelectionMenu menu, Action<IActionMetaInfo> onSelect)
        {

            foreach (var item in uFrameECS.Actions)
            {
                var item1 = item;
                if (item.Value.Category == null) continue;

                var category = menu.CreateCategoryIfNotExist(item.Value.CategoryPath.ToArray());
                category.Add(new SelectionMenuItem(item.Value, () =>
                {
                    onSelect(item1.Value);
                }));


            }
            //foreach (var action in this.Container.Resolve<IRepository>().AllOf<GraphNode>().OfType<IActionMetaInfo>())
            //{
            //    var action1 = action;
            //    menu.AddItem(new SelectionMenuItem(action, () =>
            //    {
            //        onSelect(action1);
            //    }));
            //}
            foreach (var action in this.Container.Resolve<IRepository>().AllOf<GraphNode>().OfType<IActionMetaInfo>())
            {
                var action1 = action;
                menu.AddItem(new SelectionMenuItem(action, () =>
                {
                    onSelect(action1);
                }));
            }
            //var _categoryTitles = uFrameECS.Actions
            //    .Where(_ => _.Value.Category != null)
            //    .SelectMany(_ => _.Value.Category.Title)
            //    .Distinct();

            //foreach (var categoryTitle in _categoryTitles)
            //{
            //    var category = new SelectionMenuCategory()
            //    {
            //        Title = categoryTitle
            //    };
            //    menu.AddItem(category);
            //    var title = categoryTitle;

            //    foreach (
            //        var action in uFrameECS.Actions.Values.Where(_ => _.Category != null && _.Category.Title.Contains(title)))
            //    {
            //        var action1 = action;
            //        menu.AddItem(new SelectionMenuItem(action, () =>
            //        {
            //            onSelect(action1);
            //        }), category);
            //    }
            //}
            foreach (
                var action in uFrameECS.Actions.Values.Where(_ => _.Category == null))
            {
                var action1 = action;
                menu.AddItem(new SelectionMenuItem(action, () =>
                {
                    onSelect(action1);
                }));
            }
        }

        public void GetPages(List<DocumentationPage> rootPages)
        {
            //foreach (var item in Actions)
            //{
            //    rootPages.Add(new ActionPage() { MetaInfo = item.Value });
            //}
            var configs = Container.Resolve<DatabaseService>().Configurations.Values;
            if (configs != null)
            {
                foreach (var config in configs)
                {
                    var configPage = new ConfigPage(config);
                    var componentsPage = new CategoryPage("Components/Descriptors/Groups");
                    var systemsPage = new CategoryPage("Systems");
                    var eventsPage = new CategoryPage("Events");
                    configPage.ChildPages.Add(componentsPage);
                    configPage.ChildPages.Add(systemsPage);
                    configPage.ChildPages.Add(eventsPage);
                    //var componentsPage = new ConfigPage(config);

                    foreach (var item in config.Repository.AllOf<SystemNode>())
                    {

                        //if (item.Comments != null)
                        //{ 
                        systemsPage.ChildPages.Add(new NodePage(item));
                        //}
                    }
                    //foreach (var item in config.Repository.AllOf<HandlerNode>())
                    //{
                    //    if (item.Comments != null)
                    //    {
                    //        componentsPage.ChildPages.Add(new NodePage(item));
                    //    }
                    //}
                    foreach (var item in config.Repository.AllOf<ComponentNode>())
                    {
                        componentsPage.ChildPages.Add(new NodePage(item));
                    }
                    foreach (var item in config.Repository.AllOf<GroupNode>())
                    {
                        componentsPage.ChildPages.Add(new NodePage(item));
                    }
                    foreach (var item in config.Repository.AllOf<DescriptorNode>())
                    {

                        componentsPage.ChildPages.Add(new NodePage(item));
                    }

                    foreach (var item in config.Repository.AllOf<EventNode>())
                    {
                        eventsPage.ChildPages.Add(new NodePage(item));
                    }
                    rootPages.Add(configPage);
                }
            }
        }

        public void GetDocumentation(IDocumentationBuilder node)
        {

        }
        #endregion

        #region Execute Methods
        public void Execute(AddSlotInputNodeCommand command)
        {
            //var referenceNode = new VariableReferenceNode()
            //{

            //    VariableId = command.Variable.Identifier,
            //    HandlerId = command.Handler.Identifier
            //};

            //command.DiagramViewModel.AddNode(referenceNode, command.Position);
            //var connectionData = command.DiagramViewModel.CurrentRepository.Create<ConnectionData>();
            //connectionData.InputIdentifier = command.Input.Identifier;
            //connectionData.OutputIdentifier = referenceNode.Identifier;
            //referenceNode.Name = command.Variable.VariableName;
        }

        public void Execute(NewModuleWorkspace command)
        {
            var repository = InvertApplication.Container.Resolve<IRepository>();
            var createWorkspaceCommand = new CreateWorkspaceCommand() { Name = command.Name, Title = command.Name };

            Execute(createWorkspaceCommand);


            var dataGraph = new DataGraph();
            var systemGraph = new SystemGraph();
            dataGraph.Name = command.Name + "Data";
            systemGraph.Name = command.Name + "System";

            repository.Add(dataGraph);
            repository.Add(systemGraph);

            createWorkspaceCommand.Result.AddGraph(dataGraph);
            createWorkspaceCommand.Result.AddGraph(systemGraph);
            createWorkspaceCommand.Result.CurrentGraphId = dataGraph.Identifier;

            Execute(new OpenWorkspaceCommand()
            {
                Workspace = createWorkspaceCommand.Result
            });



        }

        public void Execute(GroupActionNodes command)
        {

            List<IDiagramNodeItem> list = new List<IDiagramNodeItem>();
            GrabDependencies(list, command.Node);

            list.Add(command.Node);

            var groupNode = new ActionGroupNode();
            InvertGraphEditor.CurrentDiagramViewModel.AddNode(groupNode, command.Node.FilterLocation.Position);
            foreach (var item in list.OfType<GenericNode>())
            {
                item.FilterLocation.FilterId = groupNode.Identifier;
            }
            groupNode.IsSelected = true;
            groupNode.IsEditing = true;
            Container.Resolve<IRepository>().Add(groupNode);
        }
        #endregion

        #region Other Methods
        public void OnMouseDoubleClick(Drawer drawer, MouseEvent mouseEvent)
        {
            var d = drawer as DiagramDrawer;
            if (d != null)
            {
                // When we've clicked nothing
                if (d.DrawersAtMouse.Length < 1)
                {
                    LastMouseEvent = mouseEvent;
                    //InvertApplication.SignalEvent<IWindowsEvents>(_ =>
                    //{
                    //    _.ShowWindow("QuickAccessWindowFactory", "Add Node", null, mouseEvent.LastMousePosition,
                    //        new Vector2(500, 600));
                    //});
                    ShowQuickAccess(mouseEvent);
                }
                else
                {

                }
                var firstOrDefault = d.DrawersAtMouse.FirstOrDefault();
                if (firstOrDefault != null)
                {
                    var handlerVM = firstOrDefault.ViewModelObject as HandlerNodeViewModel;
                    if (handlerVM != null)
                    {
                        if (false)
                        {
                            var config = InvertGraphEditor.Container.Resolve<IGraphConfiguration>();
                            var fileGenerators = InvertGraphEditor.GetAllFileGenerators(config, new[] { handlerVM.DataObject as IDataRecord }).ToArray();
                            var editableGenerator = fileGenerators.FirstOrDefault(p => p.Generators.Any(x => !x.AlwaysRegenerate));
                            if (editableGenerator != null)
                                InvertGraphEditor.Platform.OpenScriptFile(editableGenerator.AssetPath);
                        }
                    }
                }
            }
        }

        private void ShowQuickAccess(MouseEvent mouseEvent)
        {
            if (InvertGraphEditor.CurrentDiagramViewModel != null)
            {
                var items = InvertGraphEditor.CurrentDiagramViewModel.SelectedNodeItems.ToArray();
                foreach (var item in items)
                {
                    item.IsSelected = false;
                }
            }
            var menu = new SelectionMenu();

            QueryInsert(menu);

            InvertApplication.SignalEvent<IShowSelectionMenu>(_ => _.ShowSelectionMenu(menu, mouseEvent.LastMousePosition - mouseEvent.ContextScroll - new Vector2(20, 0)));
            //InvertApplication.SignalEvent<IShowSelectionMenu>(_ => _.ShowSelectionMenu(new QuickAccessContext()
            //{
            //    ContextType = typeof(IInsertQuickAccessContext),
            //    MouseData = mouseEvent
            //}, mouseEvent.LastMousePosition));

        }

        public void GrabDependencies(List<IDiagramNodeItem> items, GraphNode node)
        {
            foreach (var item in node.GraphItems.OfType<IConnectable>())
            {

                foreach (var dependent in item.InputsFrom<IDiagramNodeItem>().Concat(item.OutputsTo<IDiagramNodeItem>()))
                {
                    if (items.Contains(dependent)) continue;
                    if (items.Contains(dependent.Node)) continue;

                    items.Add(dependent);
                    items.Add(dependent.Node);

                    GrabDependencies(items, dependent.Node);

                }

            }
        }

        public void UpgradeDatabase(uFrameDatabaseConfig item)
        {

            if (item.BuildNumber < 1)
            {
                item.BuildNumber = 1;
                item.Repository.Commit();
                InvertApplication.Log("Updating database.  You should commit changes to any version control.");
                if (InvertGraphEditor.Platform.MessageBox("Regenerate System Files", "IMPORTANT! This will delete all system node files and regenerate them, if you already have custom system code (most likely pro users only), then click no.", "OK",
                    "Nope, I've already fixed my systems."))
                {
                    var systemNodes = item.Database.AllOf<SystemNode>().ToArray();
                    foreach (var sn in systemNodes)
                    {
                        foreach (var f in sn.GetAllEditableFilesForNode(item.Database.GetSingle<uFrameDatabaseConfig>())
                            .Where(p => File.Exists(p.FullPathName)))
                        {
                            InvertApplication.Log(string.Format("Removing file {0} for recompile.", f.FullPathName));
                            File.Delete(f.FullPathName);
                        }
                    }

                }

                //Execute(new SaveAndCompileCommand() { ForceCompileAll = true });

            }
        }

        public void CompilingStarted(IRepository repository)
        {
            var items = repository.AllOf<IComponentId>().OrderBy(p => p.ComponentId).ToArray();
            for (int index = 0; index < items.Length; index++)
            {
                var item = items[index];
                item.ComponentId = index + 1;
            }
            var eventIds = repository.AllOf<IEventId>().OrderBy(p => p.EventId).ToArray();
            for (int index = 0; index < eventIds.Length; index++)
            {
                var item = eventIds[index];
                item.EventId = index + 1;
            }

            repository.Commit();
        }

        public void SelectionChanged(GraphItemViewModel selected)
        {
            if (uFrameHelp.Instance != null)
            {
                var item =
                    uFrameHelp.Instance.AllPages().OfType<NodePage>().FirstOrDefault(p => p.Node == selected.DataObject);
                if (item == null)
                {

                }

                if (item != null)
                {
                    uFrameHelp.Instance.PageStack.Push(item);
                    uFrameHelp.Instance.Repaint();
                }

            }
        }

        
        #endregion 
    }

    #region Interface
    public interface IQueryActionMetaInfo
    {
        void QueryActions(List<IActionMetaInfo> actions);
    }
    #endregion

    #region DiagramPlugin
    public class ActionClassImporter : DiagramPlugin, IQueryActionMetaInfo
    {
        public IEnumerable<Type> ActionTypes
        {
            get
            {
                foreach (var assembly in InvertApplication.TypeAssemblies)
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.IsDefined(typeof(ActionTitle), true))
                        {
                            yield return type;
                        }
                    }
                }
            }
        }
        public void QueryActions(List<IActionMetaInfo> actions)
        {
            foreach (var actionType in ActionTypes)
            {
                var actionInfo = new ActionMetaInfo(actionType);
                var descAttrib = actionType.GetCustomAttributes(typeof(ActionDescription), true).OfType<ActionDescription>().FirstOrDefault();
                actionInfo.DescriptionAttribute = descAttrib;
                actionInfo.MetaAttributes =
                    actionType.GetCustomAttributes(typeof(ActionMetaAttribute), true).OfType<ActionMetaAttribute>().ToArray();
                var fields = actionType.GetFields(BindingFlags.Instance | BindingFlags.Public);
                if (!typeof(SequenceItemNode).IsAssignableFrom(actionType))
                {
                    foreach (var field in fields)
                    {
                        var paramOpt = field.GetCustomAttributes(typeof(Optional), true).OfType<Optional>().FirstOrDefault();
                        var fieldMetaInfo = new ActionFieldInfo()
                        {
                            MemberType = new SystemTypeInfo(field.FieldType),
                            Name = field.Name,
                            IsBranch = field.FieldType.IsSubclassOf(typeof(Delegate)),
                            MemberName = field.Name,
                            IsOptional = paramOpt != null
                        };

                        if (!uFrameECS.SystemTypes.Contains(field.FieldType))
                            uFrameECS.SystemTypes.Add(field.FieldType);

                        fieldMetaInfo.MetaAttributes =
                            field.GetCustomAttributes(typeof(ActionAttribute), true)
                                .OfType<ActionAttribute>()
                                .ToArray();
                        if (fieldMetaInfo.DisplayType == null)
                            continue;

                        actionInfo.ActionFields.Add(fieldMetaInfo);
                    }
                }
                else
                {
                    Container.RegisterRelation(actionType, typeof(ViewModel), typeof(CustomActionViewModel));
                    Container.GetNodeConfig(actionType);
                    actionInfo.IsEditorClass = true;
                }

                actions.Add(actionInfo);
            }
        }
    }

    public class ActionLibraryImporter : DiagramPlugin, IQueryActionMetaInfo, IDataRecordInserted, IDataRecordPropertyChanged, IDataRecordRemoved
    {
        private static HashSet<Type> _staticLibraries;

        public override void Initialize(UFrameContainer container)
        {
            base.Initialize(container);
            StaticLibraries.Add(typeof(Input));
            StaticLibraries.Add(typeof(Math));
            StaticLibraries.Add(typeof(Mathf));
            StaticLibraries.Add(typeof(Physics));
            StaticLibraries.Add(typeof(Physics2D));
            StaticLibraries.Add(typeof(PlayerPrefs));
            StaticLibraries.Add(typeof(Application));
        }

        public static HashSet<Type> StaticLibraries
        {
            get { return _staticLibraries ?? (_staticLibraries = new HashSet<Type>()); }
            set { _staticLibraries = value; }
        }

        public void QueryActions(List<IActionMetaInfo> actions)
        {

            foreach (var type in GetLibraryTypes())
            {

                var category = type.GetCustomAttributes(typeof(uFrameCategory), true).OfType<uFrameCategory>().FirstOrDefault();
                var descAttrib = type.GetCustomAttributes(typeof(ActionDescription), true).OfType<ActionDescription>().FirstOrDefault();
                var title = type.GetCustomAttributes(typeof(ActionTitle), true).OfType<ActionTitle>().FirstOrDefault();
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var method in methods)
                {
                    if (method.Name.StartsWith("get_")) continue;
                    if (method.Name.StartsWith("set_")) continue;
                    var actionInfo = new ActionMethodMetaInfo(type)
                    {
                        Category = category,
                        DescriptionAttribute = descAttrib,
                        Method = method
                    };

                    actionInfo.MetaAttributes =
                        method.GetCustomAttributes(typeof(ActionMetaAttribute), true)
                            .OfType<ActionMetaAttribute>()
                            .ToArray();

                    if (actionInfo.Category == null)
                    {
                        actionInfo.Category = new uFrameCategory(type.Name);
                    }
                    //var genericArguments = method.GetGenericArguments();
                    var vars = method.GetParameters();

                    if (!method.IsStatic)
                    {
                        var fieldMetaInfo = new ActionFieldInfo()
                        {
                            MemberType = new SystemTypeInfo(type),
                            Name = "Instance",
                            IsBranch = false,
                            MemberName = "Instance",
                            IsByRef = false,
                            DisplayType = new In() { DisplayName = "Instance", IsNewLine = true, ParameterName = "Instance" }
                        };

                        actionInfo.InstanceInfo = fieldMetaInfo;
                        actionInfo.ActionFields.Add(fieldMetaInfo);
                    }
                    foreach (var parameter in vars)
                    {
                        var fieldMetaInfo = new ActionFieldInfo()
                        {
                            MemberType = new SystemTypeInfo(parameter.ParameterType),
                            Name = parameter.Name,
                            IsBranch = parameter.ParameterType.IsSubclassOf(typeof(Delegate)),
                            MemberName = parameter.Name,
                            IsByRef = parameter.ParameterType.IsByRef
                        };

                        if (!uFrameECS.SystemTypes.Contains(parameter.ParameterType))
                            uFrameECS.SystemTypes.Add(parameter.ParameterType);

                        //  Should these be part of the action meta info class ? - Micah Oct-15-2015
                        var paramDescr = parameter.GetCustomAttributes(typeof(Description), true).OfType<Description>().FirstOrDefault();
                        var paramOpt = parameter.GetCustomAttributes(typeof(Optional), true).OfType<Optional>().FirstOrDefault();
                        if (paramDescr != null) fieldMetaInfo.Description = paramDescr.Text;
                        if (paramOpt != null) fieldMetaInfo.IsOptional = true;


                        fieldMetaInfo.MetaAttributes =
                            parameter.GetCustomAttributes(typeof(ActionAttribute), true)
                                .Cast<ActionAttribute>().ToArray();

                        //if (!fieldMetaInfo.MetaAttributes.Any())
                        //{
                        if (parameter.IsOut || fieldMetaInfo.IsBranch)
                        {
                            fieldMetaInfo.DisplayType = new Out(parameter.Name, parameter.Name);


                        }
                        else
                        {
                            fieldMetaInfo.DisplayType = new In(parameter.Name, parameter.Name);
                        }

                        //}
                        actionInfo.ActionFields.Add(fieldMetaInfo);
                        if (fieldMetaInfo.IsBranch)
                        {
                            var parameters = parameter.ParameterType.GetMethod("Invoke").GetParameters();
                            foreach (var p in parameters)
                            {
                                var result = new ActionFieldInfo()
                                {
                                    MemberType = new SystemTypeInfo(p.ParameterType),
                                    IsReturn = false,
                                    Name = p.Name,
                                    MemberName = p.Name,
                                    IsDelegateMember = true
                                };

                                //result.MetaAttributes =
                                //    method.GetCustomAttributes(typeof (FieldDisplayTypeAttribute), true)
                                //        .OfType<FieldDisplayTypeAttribute>().ToArray();

                                result.DisplayType = new Out(p.Name, p.Name);
                                actionInfo.ActionFields.Add(result);
                            }

                        }
                    }
                    if (method.ReturnType != typeof(void))
                    {
                        var result = new ActionFieldInfo()
                        {
                            MemberType = new SystemTypeInfo(method.ReturnType),
                            IsReturn = true,
                            Name = "Result",
                            MemberName = "Result"
                        };
                        if (!uFrameECS.SystemTypes.Contains(method.ReturnType))
                            uFrameECS.SystemTypes.Add(method.ReturnType);

                        result.MetaAttributes =
                            method.GetCustomAttributes(typeof(FieldDisplayTypeAttribute), true)
                                .OfType<FieldDisplayTypeAttribute>()
                                .Where(p => p.ParameterName == "Result").ToArray();

                        result.DisplayType = new Out("Result", "Result");
                        actionInfo.ActionFields.Add(result);
                    }

                    actions.Add(actionInfo);
                }

            }
        }

        private IEnumerable<Type> GetLibraryTypes()
        {
            foreach (var assembly in InvertApplication.CachedAssemblies.Concat(InvertApplication.TypeAssemblies))
            {
                foreach (var item in assembly.GetTypes().Where(p => p.IsSealed && p.IsDefined(typeof(ActionLibrary), true) || StaticLibraries.Contains(p)))
                {
                    yield return item;
                }
            }

            var graphConfig = Container.Resolve<DatabaseService>().CurrentConfiguration;
            if (graphConfig != null)
            {
                var typeReferences = graphConfig.Repository.All<TypeReferenceNode>();
                foreach (var item in typeReferences)
                {
                    var t = item.Type;
                    if (t == null) continue;
                    yield return t;
                }
            }
        }

        public void RecordInserted(IDataRecord record)
        {
            if (record is TypeReferenceNode)
            {
                this.Container.Resolve<uFrameECS>().LoadActions();
            }
        }

        public void PropertyChanged(IDataRecord record, string name, object previousValue, object nextValue)
        {
            if (record is TypeReferenceNode)
            {
                this.Container.Resolve<uFrameECS>().LoadActions();
            }
        }

        public void RecordRemoved(IDataRecord record)
        {
            if (record is TypeReferenceNode)
            {
                this.Container.Resolve<uFrameECS>().LoadActions();
            }
        }
    }

    public class uFrameECSDescriptors : DiagramPlugin, IContextMenuQuery
    {
        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, params object[] obj)
        {
            var componentViewModel = obj.OfType<ComponentNodeViewModel>().FirstOrDefault();
            if (componentViewModel != null)
            {
                var property = componentViewModel.DataObject as GraphNode;
                var db = Container.Resolve<DatabaseService>().CurrentConfiguration;
                foreach (var item in db.Database.All<DescriptorNode>())
                {
                    var item1 = item;
                    ui.AddCommand(new ContextMenuItem()
                    {
                        Checked = property[item.Identifier],
                        Title = item.Name,
                        Group = "Descriptors",
                        Command = new LambdaCommand(item.Name, () =>
                        {
                            property[item1.Identifier] = !property[item1.Identifier];
                        })
                    });
                }
            }
            var propertyViewModel = obj.OfType<TypedItemViewModel>().FirstOrDefault();
            if (propertyViewModel != null)
            {
                var property = propertyViewModel.DataObject as GenericTypedChildItem;
                if (property != null)
                {
                    var db = Container.Resolve<DatabaseService>().CurrentConfiguration;
                    if (db != null && db.Database != null)
                    {
                        foreach (var item in db.Database.All<DescriptorNode>())
                        {
                            var item1 = item;
                            ui.AddCommand(new ContextMenuItem()
                            {
                                Checked = property[item.Identifier],
                                Title = item.Name,
                                Group = "Descriptors",
                                Command = new LambdaCommand(item.Name, () =>
                                {
                                    property[item1.Identifier] = !property[item1.Identifier];
                                })
                            });
                        }
                    }
                }
            }
        }
    }
    #endregion

    public class ECSConnectionStrategy : DefaultConnectionStrategy
    {
        //protected override bool CanConnect(EventNode output, HandlerNode input)
        //{
        //    if (input.Meta == output)
        //    {
        //        return true;
        //    }
        //    return base.CanConnect(output, input);
        //}

        //public override bool IsConnected(IRepository currentRepository, EventNode output, HandlerNode input)
        //{
        //    if (input.Meta == output)
        //    {
        //        return true;
        //    }
        //    return base.IsConnected(currentRepository, output, input);
        //}
        public override bool IsConnected(ConnectorViewModel output, ConnectorViewModel input)
        {
            var handler = input.DataObject as HandlerNode;
            var evt = output.DataObject as EventNode;
            if (handler != null && evt != null && handler.Meta != null)
            {
                if (handler.Meta.FullName == evt.FullName)
                {
                    return true;
                }


            }

            var group = output.DataObject as IMappingsConnectable;
            if (handler != null && group != null)
            {
                if (handler.HandlerInputs.Any(p => p.Item == group))
                {
                    return true;
                }
            }

            evt = input.DataObject as EventNode;
            handler = output.DataObject as HandlerNode;

            if (handler != null && evt != null && handler.FilterNodes.OfType<PublishEventNode>().Any(p => p.Event.Item == evt))
            {
                return true;
            }



            //if (handler  != null)


            return base.IsConnected(output, input);
        }

        public override ConnectionViewModel Connect(DiagramViewModel diagramViewModel, ConnectorViewModel a, ConnectorViewModel b)
        {
            return base.Connect(diagramViewModel, a, b);
        }

        public override Color ConnectionColor { get { return new Color(0.2f, 0.2f, 0.2f, 0.4f); } }
        public override void Remove(ConnectorViewModel output, ConnectorViewModel input)
        {

        }
    }

    public class EventsExplorerProvider : IExplorerProvider
    {
        public string Name { get { return "Events"; } }
        public List<IItem> GetItems(IRepository repository)
        {
            var list = new List<IItem>();
            foreach (var item in repository.All<EventNode>())
            {
                var eventCategory = new SelectionMenuCategory()
                {
                    Title = item.Name,
                    Expanded = true
                };
                list.Add(eventCategory);

                //var listenersCategory = new SelectionMenuCategory() {Title = "Listeners", Expanded = true};
                //var publishersCategory = new SelectionMenuCategory() {Title = "Publishers", Expanded = true};



                foreach (var listener in repository.All<HandlerNode>().Where(p => p.Meta == item))
                {
                    //listenersCategory.Add(new SelectionMenuItem(string.Empty, "-> " + listener.Name, () => { }));
                    //if (!eventCategory.ChildItems.Contains(listenersCategory))
                    //{
                    //    eventCategory.Add(listenersCategory);
                    //}
                    eventCategory.Add(new SelectionMenuItem(string.Empty, "-> " + listener.Title, () => { }));
                }
                //foreach (var listener in repository.All<HandlerNode>().Where(p=>p.FilterNodes.OfType<PublishEventNode>().Any(x=>x.SelectedEvent == item)))
                //{
                //    // publishersCategory.Add(new SelectionMenuItem(string.Empty,listener.Name,()=> {}));
                //    //if (!eventCategory.ChildItems.Contains(publishersCategory))
                //    //{
                //    //    eventCategory.Add(publishersCategory);
                //    //}
                //    eventCategory.Add(new SelectionMenuItem(string.Empty, listener.Title + " -> ", () => { }));
                //}

            }
            return list;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AutoNamespaces : TemplateAttribute
    {
        public override int Priority
        {
            get { return -3; }
        }

        public override void Modify(object templateInstance, MemberInfo info, TemplateContext ctx)
        {
            var obj = ctx.DataObject as IDiagramNodeItem;
            if (obj != null)
                foreach (var item in obj.Graph.NodeItems.OfType<ISystemGroupProvider>().SelectMany(p => p.GetSystemGroups()))
                {
                    ctx.TryAddNamespace(item.Namespace);
                    foreach (var child in item.PersistedItems.OfType<IMemberInfo>())
                    {
                        ctx.TryAddNamespace(child.MemberType.Namespace);
                    }
                }
        }
    }

    // TODO : Documentation Page
    #region Documentation Page
    public class uFrameECSPage : DocumentationPage { }

    public class ActionPage : DocumentationPage
    {
        public IActionMetaInfo MetaInfo { get; set; }

        public override string Name
        {
            get { return MetaInfo.Title; }
        }

        public override void GetContent(IDocumentationBuilder _)
        {
            base.GetContent(_);

            _.Title3(string.Join(",", MetaInfo.CategoryPath.ToArray()));

            _.Break();
            _.Title2("Inputs");
            foreach (var item in MetaInfo.GetMembers().OfType<IActionFieldInfo>().Where(p => p.DisplayType is In))
            {
                _.Title3(item.Name);
                PrintDescription(_, item);
            }
            _.Break();
            _.Title2("Outputs");
            foreach (var item in MetaInfo.GetMembers().OfType<IActionFieldInfo>().Where(p => p.DisplayType is Out && !p.MemberType.IsAssignableTo(new SystemTypeInfo(typeof(Delegate)))))
            {
                _.Title3(item.Name);
                PrintDescription(_, item);
            }
            _.Break();
            _.Title2("Branches");
            foreach (var item in MetaInfo.GetMembers().OfType<IActionFieldInfo>().Where(p => p.DisplayType is Out && p.MemberType.IsAssignableTo(new SystemTypeInfo(typeof(Delegate)))))
            {
                _.Title3(item.Name);
                PrintDescription(_, item);
            }
        }

        private static void PrintDescription(IDocumentationBuilder _, IActionFieldInfo item)
        {
            var actionDescription = item.GetAttribute<ActionDescription>();
            if (actionDescription != null)
            {
                _.Paragraph(actionDescription.Description);
            }
        }
    }

    public class ConfigPage : DocumentationPage
    {
        public ConfigPage(uFrameDatabaseConfig config)
        {
            Config = config;
        }

        public override string Name { get { return Config.Title; } }

        public uFrameDatabaseConfig Config { get; set; }

    }

    public class CategoryPage : DocumentationPage
    {
        private string _nameField;

        public CategoryPage(string name)
        {
            _nameField = name;
        }

        public override string Name
        {
            get { return _nameField; }
        }

        public GenericNode Node { get; set; }
        public override void GetContent(IDocumentationBuilder _)
        {
            base.GetContent(_);
            foreach (var item in ChildPages.OfType<NodePage>())
            {

            }
        }
    }

    public class NodePage : DocumentationPage
    {
        public NodePage(GenericNode node)
        {
            Node = node;
        }
        public override string Name { get { return Node.Name; } }
        public GenericNode Node { get; set; }


        public override void GetContent(IDocumentationBuilder _)
        {
            base.GetContent(_);
            //_.Title(Node.Name);
            var r = _.EditableParagraph(Node.Comments);
            if (r != Node.Comments)
            {
                Node.Comments = r;
            }
            foreach (var item in Node.GetMembers().OfType<PropertiesChildItem>())
            {
                _.Title2(item.MemberName);

                var result = _.EditableParagraph(item.Description);
                if (result != item.Description)
                {
                    item.Description = result;
                }
                _.Break();
            }
            var handlers = Node.Children.OfType<HandlerNode>().ToArray();
            if (handlers.Length > 0)
            {
                _.Title2("Handlers");
                foreach (var item in handlers)
                {
                    if (item.Meta == null) continue;
                    _.Title3(string.Format("-> {0} -> {1}", item.Meta.Title, item.Name));

                    var result = _.EditableParagraph(item.Comments);
                    if (result != item.Comments)
                    {
                        item.Comments = result;
                    }
                    _.Break();

                }

            }
            var notes = Node.Children.OfType<NoteNode>().ToArray();
            if (notes.Length > 0)
            {

                foreach (var item in notes)
                {
                    var re = _.EditableParagraph(item.HeaderText);
                    if (re != item.HeaderText)
                    {
                        item.HeaderText = re;
                    }


                    var result = _.EditableParagraph(item.Comments);
                    if (result != item.Comments)
                    {
                        item.Comments = result;
                    }
                    _.Break();

                }

            }



        }
    }
    #endregion
}
