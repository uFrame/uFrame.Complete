using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class StartTimerNodeDrawer : GenericNodeDrawer<StartTimerNode,StartTimerNodeViewModel> {
        
        public StartTimerNodeDrawer(StartTimerNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
