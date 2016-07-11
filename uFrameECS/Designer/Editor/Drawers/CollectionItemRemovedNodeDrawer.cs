using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class CollectionItemRemovedNodeDrawer : GenericNodeDrawer<CollectionItemRemovedNode,CollectionItemRemovedNodeViewModel> {
        
        public CollectionItemRemovedNodeDrawer(CollectionItemRemovedNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
