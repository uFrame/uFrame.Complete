using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class DescriptorNodeDrawer : GenericNodeDrawer<DescriptorNode,DescriptorNodeViewModel> {
        
        public DescriptorNodeDrawer(DescriptorNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
