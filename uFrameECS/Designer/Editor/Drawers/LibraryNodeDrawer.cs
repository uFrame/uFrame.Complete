using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class LibraryNodeDrawer : GenericNodeDrawer<LibraryNode,LibraryNodeViewModel> {
        
        public LibraryNodeDrawer(LibraryNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
