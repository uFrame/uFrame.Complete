using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class AllTrueNodeDrawer : GenericNodeDrawer<AllTrueNode,AllTrueNodeViewModel> {
        
        public AllTrueNodeDrawer(AllTrueNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
