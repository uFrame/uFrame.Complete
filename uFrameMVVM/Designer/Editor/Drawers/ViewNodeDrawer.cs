using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.MVVM
{
    public class ViewNodeDrawer : GenericNodeDrawer<ViewNode,ViewNodeViewModel> {
        
        public ViewNodeDrawer(ViewNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
