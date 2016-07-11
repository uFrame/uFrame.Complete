using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class FloatNodeDrawer : GenericNodeDrawer<FloatNode,FloatNodeViewModel> {
        
        public FloatNodeDrawer(FloatNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
