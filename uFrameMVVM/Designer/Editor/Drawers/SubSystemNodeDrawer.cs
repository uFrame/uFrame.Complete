using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.MVVM
{
    public class SubSystemNodeDrawer : GenericNodeDrawer<SubSystemNode,SubSystemNodeViewModel> {
        
        public SubSystemNodeDrawer(SubSystemNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
