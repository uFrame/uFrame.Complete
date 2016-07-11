using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI.Drawers;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.Editor.GraphUI.Scaffolding
{
    public class ScaffoldNodeChildItem<TData> where TData : IDiagramNodeItem
    {
#if !SERVER
        public class Drawer : ItemDrawer
        {
            public Drawer(ViewModel viewModel)
                : base(viewModel)
            {
            }
        }

        public class ViewModel : GenericItemViewModel<TData>
        {
            public ViewModel(TData graphItemObject, DiagramNodeViewModel diagramViewModel)
                : base(graphItemObject, diagramViewModel)
            {
            }
        }
#endif
    }

}