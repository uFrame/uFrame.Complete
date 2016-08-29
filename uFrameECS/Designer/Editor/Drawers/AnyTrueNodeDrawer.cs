using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class AnyTrueNodeDrawer : GenericNodeDrawer<AnyTrueNode,AnyTrueNodeViewModel> {
        
        public AnyTrueNodeDrawer(AnyTrueNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
