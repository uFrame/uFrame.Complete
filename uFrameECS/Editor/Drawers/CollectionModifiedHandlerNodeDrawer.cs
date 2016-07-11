using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class CollectionModifiedHandlerNodeDrawer : GenericNodeDrawer<CollectionModifiedHandlerNode,CollectionModifiedHandlerNodeViewModel> {
        
        public CollectionModifiedHandlerNodeDrawer(CollectionModifiedHandlerNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
