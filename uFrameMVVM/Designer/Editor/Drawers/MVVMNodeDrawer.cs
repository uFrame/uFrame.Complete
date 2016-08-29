using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.MVVM
{
    public class MVVMNodeDrawer : GenericNodeDrawer<MVVMNode,MVVMNodeViewModel> {
        
        public MVVMNodeDrawer(MVVMNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
