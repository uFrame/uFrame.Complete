using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using uFrame.Attributes;
using uFrame.ECS.Components;
using uFrame.ECS.Editor;
using uFrame.Editor;
using uFrame.Editor.Core;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI.Drawers;
using uFrame.Editor.NavigationSystem;
using uFrame.Editor.Platform;
using uFrame.Editor.Unity;
using uFrame.IOC;
using Object = UnityEngine.Object;

namespace uFrame.ECS.Editor
{
    [UnityEditor.CustomEditor(typeof(Entity))]
    public class EntityEditor : UnityEditor.Editor
    {
        private static Dictionary<int, List<string>> _markers;
        private static IRepository _repository1;
        private static IPlatformDrawer _drawer;

        private static Dictionary<int, List<string>> Markers
        {
            get { return _markers ?? (_markers = new Dictionary<int, List<string>>()); }
            set { _markers = value; }
        }

        static EntityEditor()
        {
            // Init
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
        }

        public static IRepository Repository
        {
            get { return _repository1 ?? (_repository1 = InvertApplication.Container.Resolve<IRepository>()); }
            set { _repository1 = value; }
        }

        public static IPlatformDrawer Drawer
        {
            get { return _drawer ?? (_drawer = InvertApplication.Container.Resolve<IPlatformDrawer>()); }
        }

        static void HierarchyItemCB(int instanceID, Rect selectionRect)
        {
            var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (go == null) return;

            var components = go.GetComponents(typeof(EcsComponent));
            var iconRect = new Rect().WithSize(16, 16).InnerAlignWithUpperRight(selectionRect).AlignHorisonallyByCenter(selectionRect).Translate(-5, 0);

            foreach (var component in components)
            {
                string icon = null;

                InvertApplication.SignalEvent<IFetchIcon>(_ => icon = _.FetchIcon(component));

                if (string.IsNullOrEmpty(icon)) continue;
                var cCache = GUI.color;
                GUI.color = new Color(cCache.r, cCache.g, cCache.b, cCache.a * 0.7f);
                Drawer.DrawImage(iconRect, icon, true);
                iconRect = iconRect.LeftOf(iconRect).Translate(-5, 0);
                GUI.color = cCache;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            InvertApplication.SignalEvent<IDrawUnityInspector>(_ => _.DrawInspector(target));
        }
    }


    public interface IFetchIcon
    {
        string FetchIcon(object component);
    }

    public interface IDrawUnityInspector
    {
        void DrawInspector(Object target);
    }
    public class UnityInspectors : DiagramPlugin, IDrawUnityInspector, IDataRecordPropertyChanged, IFetchIcon
    {
        private WorkspaceService _workspaceService;

        public WorkspaceService WorkspaceService
        {
            get { return _workspaceService ?? (_workspaceService = Container.Resolve<WorkspaceService>()); }
        }
        private IRepository _repository;
        private IPlatformDrawer _drawer;
        private Dictionary<string, string> _iconsCache;
        private GUIStyle _componentEditorToolbarButtonStyle;
        //    private UserSettings _currentUser;

        public IRepository Repository
        {
            get { return _repository ?? (_repository = Container.Resolve<IRepository>()); }
        }

        public IPlatformDrawer Drawer
        {
            get { return _drawer ?? (_drawer = Container.Resolve<IPlatformDrawer>()); }
        }



        //public string CurrentUserId
        //{
        //    get { return EditorPrefs.GetString("UF_CurrentUserId", string.Empty); }
        //    set
        //    {
        //        EditorPrefs.SetString("UF_CurrentUserId",value);
        //    }
        //}

        //public UserSettings CurrentUser
        //{
        //    get { return _currentUser ?? (_currentUser = Repository.GetSingle<UserSettings>(CurrentUserId)); }
        //}

        public override void Loaded(UFrameContainer container)
        {
            base.Loaded(container);

        }


        public TreeViewModel TreeModel { get; set; }
        public void DrawInspector(Object target)
        {
            GUIHelpers.IsInsepctor = true;
            var entityComponent = target as Entity;
            if (entityComponent != null)
            {
                //if (Repository != null)
                //{
                //    EditorGUILayout.HelpBox("0 = Auto Assign At Runtime", MessageType.Info);

                //}
                //if (GUILayout.Button("Create New Component"))
                //{
                //    if (TreeModel == null)
                //    {
                //        TreeModel = new TreeViewModel()
                //        {
                //            Data = Container.Resolve<IRepository>().AllOf<ModuleNode>().Select(item=>new SelectionMenuItem(item,
                //                () =>
                //                {


                //                })).Cast<IItem>().ToList()
                //        };
                //    }
                //    else
                //    {
                //        TreeModel = null;
                //    }

                //}
                //if (TreeModel != null)
                //{
                //     var lastRect = GUILayoutUtility.GetLastRect();
                //    var rect = GUILayoutUtility.GetRect(lastRect.x, lastRect.y, 300, 500);

                //    Signal<IDrawTreeView>(_=>_.DrawTreeView(rect,TreeModel,(p,x)=>{
                //    	var item = (x as SelectionMenuItem).DataObject as IDiagramNode;
                //        Execute(new NavigateToNodeCommand()
                //        {
                //            Node = item as IDiagramNode
                //        });
                //        Execute(new CreateNodeCommand()
                //        {
                //            NodeType = typeof(ComponentNode),
                //            GraphData = InvertGraphEditor.CurrentDiagramViewModel.GraphData,

                //        });
                //        TreeModel = null;
                //    }));
                //}
            }
            var component = target as EcsComponent;
            //if (component != null)
            //{


            if (Repository != null)
            {
                var attribute = target.GetType().GetCustomAttributes(typeof(uFrameIdentifier), true).OfType<uFrameIdentifier>().FirstOrDefault();

                if (attribute != null)
                {
                    var item = Repository.GetSingle<ComponentNode>(attribute.Identifier);
                    if (component != null)
                    {

                        var inspectorBounds = new Rect(0, 0, Screen.width, Screen.height);
                        //var iconBounds = new Rect().WithSize(16, 16).InnerAlignWithUpperRight(inspectorBounds);
                        //Drawer.DrawImage(iconBounds,"CommandIcon",true);

                        //if (GUIHelpers.DoToolbarEx("System Handlers"))
                        //{
                        //    foreach (
                        //   var handlerNode in
                        //       Repository.All<HandlerNode>()
                        //           .Where(p => p.EntityGroup != null && p.EntityGroup.Item == item))
                        //    {
                        //        if (GUILayout.Button(handlerNode.Name))
                        //        {
                        //            Execute(new NavigateToNodeCommand()
                        //            {
                        //                Node = handlerNode,
                        //                Select = true
                        //            });
                        //        }

                        //    }
                        //}
                        if (GUIHelpers.DoToolbarEx("uFrame Designer"))
                        {
                            var toolbarButton = ComponentEditorToolbarButtonStyle;

                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("Serving Handlers:");
                            EditorGUILayout.EndHorizontal();

                            foreach (
                       var handlerNode in
                           Repository.All<HandlerNode>()
                               .Where(p => p.HandlerInputs.Any(x => x.Item != null && x.Item.SelectComponents.Contains(item))))
                            {

                                EditorGUILayout.BeginHorizontal();

                                var text = handlerNode.Name;
                                if (GUILayout.Button(text, toolbarButton))
                                {
                                    Execute(new NavigateToNodeCommand()
                                    {
                                        Node = handlerNode,
                                        Select = true
                                    });
                                }

                                var descRect = GUILayoutUtility.GetLastRect();


                                //GUILayout.FlexibleSpace();

                                var meta = handlerNode.Meta as EventMetaInfo;
                                if (meta != null && meta.Dispatcher &&
                                    component.gameObject.GetComponent(meta.SystemType) == null)
                                {
                                    Drawer.DrawImage(new Rect().WithSize(16, 16).InnerAlignWithCenterLeft(descRect).Translate(4, 0), "RedDotIcon", true);

                                    var cb = GUI.backgroundColor;
                                    GUI.backgroundColor = CachedStyles.GetColor(NodeColor.Carrot);
                                    //if (GUILayout.Button("+ " + meta.SystemType.Name,EditorStyles.toolbarButton))
                                    if (GUILayout.Button(new GUIContent("Add", string.Format("Add {0} which is used to invoke the corresponding event", meta.SystemType.Name)), EditorStyles.toolbarButton, GUILayout.Width(60)))
                                    {

                                        component.gameObject.AddComponent(meta.SystemType);
                                    }
                                    GUI.backgroundColor = cb;

                                }
                                else
                                {
                                    Drawer.DrawImage(new Rect().WithSize(16, 16).InnerAlignWithCenterLeft(descRect).Translate(4, 0), "GreenDotIcon", true);

                                }

                                EditorGUILayout.EndHorizontal();

                            }

                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("");
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            var cachedBackgroundColor = GUI.backgroundColor;
                            GUI.backgroundColor = CachedStyles.GetColor(NodeColor.SgiTeal);
                            if (GUILayout.Button("Edit In Designer", EditorStyles.toolbarButton))
                            {
                                Execute(new NavigateToNodeCommand()
                                {
                                    Node = item,
                                    Select = true
                                });
                            }
                            GUI.backgroundColor = cachedBackgroundColor;
                            EditorGUILayout.EndHorizontal();
                        }



                    }

                }
            }

            //}

        }

        public GUIStyle ComponentEditorToolbarButtonStyle
        {
            get
            {
                return _componentEditorToolbarButtonStyle ??
                       (_componentEditorToolbarButtonStyle = new GUIStyle(EditorStyles.toolbarButton)
                       {
                           alignment = TextAnchor.MiddleLeft,
                           padding = new RectOffset(24, 0, 0, 0)
                       });
            }
            set { _componentEditorToolbarButtonStyle = value; }
        }

        //public class UserSettings : IDataRecord
        //{
        //    [IDataRecord]
        //    public string UserName { get; set; }

        //    public int EntityId { get; set; }

        //    public int StartingId { get; }

        //    public string Identifier { get; set; }
        //    public IRepository Repository { get; set; }
        //    public bool Changed { get; set; }
        //    public IEnumerable<string> ForeignKeys { get { yield break; } }
        //}
        public void PropertyChanged(IDataRecord record, string name, object previousValue, object nextValue)
        {
            var typedRecord = record as ComponentNode;
            if (typedRecord != null && name == "CustomIcon")
            {
                var typeName = typedRecord.TypeName;
                IconsCache[typeName] = (string)nextValue;
            }
        }

        public Dictionary<string, string> IconsCache
        {
            get { return _iconsCache ?? (_iconsCache = new Dictionary<string, string>()); }
            set { _iconsCache = value; }
        }

        public string FetchIcon(object component)
        {
            string icon;
            var type = component.GetType();
            var typeName = type.Name;
            if (IconsCache.TryGetValue(typeName, out icon))
                return icon;

            if (Repository != null)
            {
                var attribute =
                    type
                        .GetCustomAttributes(typeof(uFrameIdentifier), true)
                        .OfType<uFrameIdentifier>()
                        .FirstOrDefault();

                if (attribute != null)
                {
                    var item = Repository.GetSingle<ComponentNode>(attribute.Identifier);
                    if (IconsCache != null && item != null) IconsCache[typeName] = item.CustomIcon;
                }
            }

            IconsCache.TryGetValue(typeName, out icon);
            return icon;
        }
    }
}