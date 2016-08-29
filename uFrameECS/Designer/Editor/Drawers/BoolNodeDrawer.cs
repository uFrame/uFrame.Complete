using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class BoolNodeDrawer : GenericNodeDrawer<BoolNode,BoolNodeViewModel> {
        
        public BoolNodeDrawer(BoolNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
