using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class CollectionItemAddedNodeDrawer : GenericNodeDrawer<CollectionItemAddedNode,CollectionItemAddedNodeViewModel> {
        
        public CollectionItemAddedNodeDrawer(CollectionItemAddedNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
