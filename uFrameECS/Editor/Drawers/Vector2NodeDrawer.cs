using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class Vector2NodeDrawer : GenericNodeDrawer<Vector2Node,Vector2NodeViewModel> {
        
        public Vector2NodeDrawer(Vector2NodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
