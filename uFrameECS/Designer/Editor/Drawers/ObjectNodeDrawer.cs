using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class ObjectNodeDrawer : GenericNodeDrawer<ObjectNode,ObjectNodeViewModel> {
        
        public ObjectNodeDrawer(ObjectNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
