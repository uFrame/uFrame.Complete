using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class UserMethodNodeDrawer : GenericNodeDrawer<UserMethodNode,UserMethodNodeViewModel> {
        
        public UserMethodNodeDrawer(UserMethodNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
