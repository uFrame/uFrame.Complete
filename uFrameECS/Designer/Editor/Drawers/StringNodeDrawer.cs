using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class StringNodeDrawer : GenericNodeDrawer<StringNode,StringNodeViewModel> {
        public override float Width
        {
            get { return 500f; }
        }

        public StringNodeDrawer(StringNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
