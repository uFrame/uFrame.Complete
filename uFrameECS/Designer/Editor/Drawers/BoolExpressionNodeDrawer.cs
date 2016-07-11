using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class BoolExpressionNodeDrawer : GenericNodeDrawer<BoolExpressionNode,BoolExpressionNodeViewModel> {
        
        public BoolExpressionNodeDrawer(BoolExpressionNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
