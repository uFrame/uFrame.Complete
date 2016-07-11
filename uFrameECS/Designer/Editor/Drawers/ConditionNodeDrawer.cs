using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class ConditionNodeDrawer : GenericNodeDrawer<ConditionNode,ConditionNodeViewModel> {
        
        public ConditionNodeDrawer(ConditionNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
