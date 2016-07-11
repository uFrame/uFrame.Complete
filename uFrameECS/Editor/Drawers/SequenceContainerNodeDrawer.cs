using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class SequenceContainerNodeDrawer : GenericNodeDrawer<SequenceContainerNode,SequenceContainerNodeViewModel> {
        
        public SequenceContainerNodeDrawer(SequenceContainerNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
