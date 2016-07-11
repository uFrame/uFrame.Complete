using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class NullNodeDrawer : GenericNodeDrawer<NullNode,NullNodeViewModel> {
        
        public NullNodeDrawer(NullNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
