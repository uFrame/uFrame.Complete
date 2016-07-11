using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class FunctionNodeDrawer : GenericNodeDrawer<FunctionNode,FunctionNodeViewModel> {
        
        public FunctionNodeDrawer(FunctionNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
