using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class Vector3NodeDrawer : GenericNodeDrawer<Vector3Node,Vector3NodeViewModel> {
        
        public Vector3NodeDrawer(Vector3NodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
