using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class AllFalseNodeDrawer : GenericNodeDrawer<AllFalseNode,AllFalseNodeViewModel> {
        
        public AllFalseNodeDrawer(AllFalseNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
