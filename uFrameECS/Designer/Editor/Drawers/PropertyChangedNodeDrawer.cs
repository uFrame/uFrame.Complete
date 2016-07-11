using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class PropertyChangedNodeDrawer : GenericNodeDrawer<PropertyChangedNode,PropertyChangedNodeViewModel> {
        
        public PropertyChangedNodeDrawer(PropertyChangedNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
