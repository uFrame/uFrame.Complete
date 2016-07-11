using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class EventNodeDrawer : GenericNodeDrawer<EventNode,EventNodeViewModel> {
        
        public EventNodeDrawer(EventNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
