using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.MVVM
{
    public class SimpleClassNodeDrawer : GenericNodeDrawer<SimpleClassNode,SimpleClassNodeViewModel> {
        
        public SimpleClassNodeDrawer(SimpleClassNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
