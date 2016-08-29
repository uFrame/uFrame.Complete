using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class ColorNodeDrawer : GenericNodeDrawer<ColorNode,ColorNodeViewModel> {
        
        public ColorNodeDrawer(ColorNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
