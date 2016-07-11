using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.MVVM
{
    public class ServiceNodeDrawer : GenericNodeDrawer<ServiceNode,ServiceNodeViewModel> {
        
        public ServiceNodeDrawer(ServiceNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
