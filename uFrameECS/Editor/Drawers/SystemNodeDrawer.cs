using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class SystemNodeDrawer : GenericNodeDrawer<SystemNode,SystemNodeViewModel> {
        
        public SystemNodeDrawer(SystemNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
