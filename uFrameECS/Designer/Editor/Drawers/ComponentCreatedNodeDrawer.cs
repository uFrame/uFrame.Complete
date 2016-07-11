using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class ComponentCreatedNodeDrawer : GenericNodeDrawer<ComponentCreatedNode,ComponentCreatedNodeViewModel> {
        
        public ComponentCreatedNodeDrawer(ComponentCreatedNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
