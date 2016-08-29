using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class IntNodeDrawer : GenericNodeDrawer<IntNode,IntNodeViewModel> {
        
        public IntNodeDrawer(IntNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
