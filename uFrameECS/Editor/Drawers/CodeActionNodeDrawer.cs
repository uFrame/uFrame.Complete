using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class CodeActionNodeDrawer : GenericNodeDrawer<CodeActionNode,CodeActionNodeViewModel> {
        
        public CodeActionNodeDrawer(CodeActionNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
