using System.Collections.Generic;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Workspaces.Data;
using uFrame.Kernel.Collection;

namespace uFrame.Editor.GraphUI.ViewModels
{
    public class DesignerViewModel : ViewModel<Workspace>
    {
        private ObservableCollection<TabViewModel> _tabs;
 
        private WorkspaceService _workspaceService;

        public override void DataObjectChanged()
        {
            base.DataObjectChanged();
            //Tabs = Data.OpenGraphs;
        }

        public WorkspaceService WorkspaceService
        {
            get { return _workspaceService ?? (_workspaceService = InvertGraphEditor.Container.Resolve<WorkspaceService>()); }
        }
        public override object DataObject
        {
            get { return WorkspaceService.CurrentWorkspace; }
            set { base.DataObject = value; }
        }

        public TabViewModel CurrentTab { get; set; }

        public IEnumerable<IGraphData> Tabs
        {
            get { return Data.Graphs; }
        }

        public void OpenTab(IGraphData graphData, string[] path = null)
        {
            Data.CurrentGraph = graphData;
        }


    }
}