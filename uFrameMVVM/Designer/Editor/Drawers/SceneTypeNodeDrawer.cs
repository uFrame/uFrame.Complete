using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.MVVM
{
    public class SceneTypeNodeDrawer : GenericNodeDrawer<SceneTypeNode,SceneTypeNodeViewModel> {
        
        public SceneTypeNodeDrawer(SceneTypeNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
