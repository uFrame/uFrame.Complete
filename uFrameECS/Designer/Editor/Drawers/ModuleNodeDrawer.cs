using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class ModuleNodeDrawer : GenericNodeDrawer<ModuleNode,ModuleNodeViewModel> {
        
        public ModuleNodeDrawer(ModuleNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
