using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class StringLiteralNodeDrawer : GenericNodeDrawer<StringLiteralNode,StringLiteralNodeViewModel> {
        
        public StringLiteralNodeDrawer(StringLiteralNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
