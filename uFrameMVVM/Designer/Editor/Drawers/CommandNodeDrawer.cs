using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.MVVM
{
    public class CommandNodeDrawer : GenericNodeDrawer<CommandNode,CommandNodeViewModel> {
        
        public CommandNodeDrawer(CommandNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
