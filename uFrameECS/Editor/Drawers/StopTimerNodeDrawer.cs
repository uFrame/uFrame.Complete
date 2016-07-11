using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class StopTimerNodeDrawer : GenericNodeDrawer<StopTimerNode, StopTimerNodeViewModel>
    {

        public StopTimerNodeDrawer(StopTimerNodeViewModel viewModel) :
                base(viewModel)
        {
        }
    }
}
