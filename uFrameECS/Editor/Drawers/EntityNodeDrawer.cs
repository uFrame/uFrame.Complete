using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class EntityNodeDrawer : GenericNodeDrawer<EntityNode,EntityNodeViewModel> {
        
        public EntityNodeDrawer(EntityNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
