using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class AnyFalseNodeDrawer : GenericNodeDrawer<AnyFalseNode,AnyFalseNodeViewModel> {
        
        public AnyFalseNodeDrawer(AnyFalseNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
