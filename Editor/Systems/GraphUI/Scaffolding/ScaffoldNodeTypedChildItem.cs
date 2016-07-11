using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI.Drawers;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.Editor.GraphUI.Scaffolding
{
    public class ScaffoldNodeTypedChildItem<TData> where TData : ITypedItem
    {
#if !SERVER
        public class Drawer : TypedItemDrawer
        {
            public Drawer(ViewModel viewModel)
                : base(viewModel)
            {
            }
        }

        public class ViewModel : TypedItemViewModel
        {
            public override string TypeLabel
            {
                get { return Data.RelatedTypeName; }
            }

            public ViewModel(TData graphItemObject, DiagramNodeViewModel diagramViewModel)
                : base(graphItemObject, diagramViewModel)
            {
            }
        }
#endif

    }
}