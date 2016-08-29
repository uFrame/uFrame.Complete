using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    using Editor;

    public class ActionGroupNodeDrawer : GenericNodeDrawer<ActionGroupNode,ActionGroupNodeViewModel> {
        
        public ActionGroupNodeDrawer(ActionGroupNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
