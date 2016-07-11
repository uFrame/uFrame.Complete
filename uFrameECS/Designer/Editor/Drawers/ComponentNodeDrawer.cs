using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class ComponentNodeDrawer : GenericNodeDrawer<ComponentNode,ComponentNodeViewModel> {
        
        public ComponentNodeDrawer(ComponentNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
