using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class LiteralNodeDrawer : GenericNodeDrawer<LiteralNode,LiteralNodeViewModel> {
        
        public LiteralNodeDrawer(LiteralNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
