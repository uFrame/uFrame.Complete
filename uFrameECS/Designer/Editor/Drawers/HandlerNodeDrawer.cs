using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class HandlerNodeDrawer : GenericNodeDrawer<HandlerNode,HandlerNodeViewModel> {
  
        public HandlerNodeDrawer(HandlerNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
