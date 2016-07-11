using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class ComponentDestroyedNodeDrawer : GenericNodeDrawer<ComponentDestroyedNode,ComponentDestroyedNodeViewModel> {
        
        public ComponentDestroyedNodeDrawer(ComponentDestroyedNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
