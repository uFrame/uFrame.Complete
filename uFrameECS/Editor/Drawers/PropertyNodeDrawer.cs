using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class PropertyNodeDrawer : GenericNodeDrawer<PropertyNode,PropertyNodeViewModel> {
        
        public PropertyNodeDrawer(PropertyNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
