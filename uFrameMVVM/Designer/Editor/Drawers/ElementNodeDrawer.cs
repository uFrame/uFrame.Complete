using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.MVVM
{
    public class ElementNodeDrawer : GenericNodeDrawer<ElementNode,ElementNodeViewModel> {
        
        public ElementNodeDrawer(ElementNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
