using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using uFrame.Editor.Database.Data;
using uFrame.ECS.Editor;
using uFrame.Editor.Core;
using uFrame.Editor.Menus;
using uFrame.Editor.NavigationSystem;
using uFrame.Editor.Platform;
using uFrame.Editor.Unity;
using uFrame.IOC;

//namespace Assets.Plugins.Editor.uFrame.ECS.Editor.DebugSystem
namespace uFrame.Editor.DebugSystem
{
    public class uFrameBreakpointsWindowPlugin : DiagramPlugin
        , IToolbarQuery
        , IDataRecordInserted
        , IDataRecordRemoved
        , IDataRecordManagerRefresh
        , IDrawBreakpointsExplorer
    {
        private IPlatformDrawer _platformDrawer;
        private IRepository _repository;
        private List<Breakpoint> _breakpoints;
        private Vector2 _scrollPosition;

        public IPlatformDrawer PlatformDrawer
        {
            get { return _platformDrawer ?? (_platformDrawer = Container.Resolve<IPlatformDrawer>()); }
            set { _platformDrawer = value; }
        }

        public IRepository Repository
        {
            get { return _repository ?? (_repository = Container.Resolve<IRepository>()); }
            set { _repository = value; }
        }

        public void QueryToolbarCommands(ToolbarUI ui)
        {
            var isBreakpointsWindowOpened = IsWindowOpen<uFrameBreakpointsWindow>();
            ui.AddCommand(new ToolbarItem()
            {
                Title = "Breakpoints",
                Checked = isBreakpointsWindowOpened,
                Position = ToolbarPosition.BottomRight,
                Command = new LambdaCommand("Show", () =>
                {
                    var window = EditorWindow.GetWindow<uFrameBreakpointsWindow>();
                    window.title = "Breakpoints";
                    if (isBreakpointsWindowOpened)
                    {
                        window.Close();
                    }
                })
            });

        }

        static bool IsWindowOpen<WindowType>() where WindowType : EditorWindow
        {
            WindowType[] windows = Resources.FindObjectsOfTypeAll<WindowType>();
            return windows != null && windows.Length > 0;

        }

        public void RecordInserted(IDataRecord record)
        {
            UpdateBreakpoints();
        }

        public void RecordRemoved(IDataRecord record)
        {
            UpdateBreakpoints();
        }

        public void ManagerRefreshed(IDataRecordManager manager)
        {
            UpdateBreakpoints();
        }

        public override void Loaded(UFrameContainer container)
        {
            base.Loaded(container);
            UpdateBreakpoints();
        }

        private void UpdateBreakpoints()
        {
            if (Repository == null) return;
            var bps = Repository.All<Breakpoint>();
            Breakpoints = bps.ToList();
            if(BreakpointsList == null)BreakpointsList= new TreeViewModel();
            BreakpointsList.SingleItemIcon = "BreakpointIcon";
            BreakpointsList.Data = Breakpoints.OfType<IItem>().ToList();
            BreakpointsList.Submit = i =>
            {
                var bp = i as Breakpoint;
                if (bp != null)
                {
                    Execute(new NavigateToNodeCommand()
                    {
                        Node = bp.Action,
                        Select = true
                    });
                }
            };
        }

        public List<Breakpoint> Breakpoints
        {
            get { return _breakpoints ?? (_breakpoints = new List<Breakpoint>()); }
            set { _breakpoints = value; }
        }

        public TreeViewModel BreakpointsList { get; set; }

        public void DrawBreakpointsExplorer(Rect rect)
        {
            if (BreakpointsList == null) return;
            if (BreakpointsList.IsDirty) BreakpointsList.Refresh();
            Signal<IDrawTreeView>(_ => _.DrawTreeView(rect, BreakpointsList, (m,i) =>
            {
                var bp = i as Breakpoint;
                if (bp != null)
                {
                    Execute(new NavigateToNodeCommand()
                    {
                        Node = bp.Action,
                        Select = true
                    });
                }
            }));

        }
    }


    public class uFrameBreakpointsWindow : EditorWindow
    {
        private Vector2 _scrollPosition;

        [MenuItem("Window/uFrame/Breakpoints #&o")]
        internal static void ShowWindow()
        {
            var window = GetWindow<uFrameBreakpointsWindow>();
            window.title = "Breakpoints";
            Instance = window;
            window.Show();
        }

        public static uFrameBreakpointsWindow Instance { get; set; }

        public void OnGUI()
        {
            Instance = this;
            var rect = new Rect(0f, 0f, Screen.width, Screen.height);

            GUILayout.BeginArea(rect);
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            InvertApplication.SignalEvent<IDrawBreakpointsExplorer>(_ => _.DrawBreakpointsExplorer(rect));
            GUILayout.EndScrollView();
            GUILayout.EndArea();

        }

        public void Update()
        {
            Repaint();
        }
    }

    public interface IDrawBreakpointsExplorer
    {
        void DrawBreakpointsExplorer(Rect rect);
    }
}
