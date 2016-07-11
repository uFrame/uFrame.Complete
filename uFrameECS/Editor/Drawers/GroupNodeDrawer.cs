using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class GroupNodeDrawer : GenericNodeDrawer<GroupNode, GroupNodeViewModel>
    {
        
        public GroupNodeDrawer(GroupNodeViewModel viewModel) : 
                base(viewModel) {
        }
   
    }
}
