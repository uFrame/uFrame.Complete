using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class VariableNodeDrawer : GenericNodeDrawer<VariableNode,VariableNodeViewModel> {
        
        public VariableNodeDrawer(VariableNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
