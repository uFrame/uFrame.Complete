using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class EnumValueNodeDrawer : GenericNodeDrawer<EnumValueNode,EnumValueNodeViewModel> {
        
        public EnumValueNodeDrawer(EnumValueNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
