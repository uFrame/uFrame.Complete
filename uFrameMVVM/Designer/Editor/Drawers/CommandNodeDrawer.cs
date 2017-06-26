using uFrame.Editor.GraphUI.Drawers;
using uFrame.Editor.Input;
using uFrame.Editor.Platform;
using UnityEngine;

namespace uFrame.MVVM
{
    public class CommandNodeDrawer : GenericNodeDrawer<CommandNode,CommandNodeViewModel>
    {

        public CommandNodeDrawer(CommandNodeViewModel viewModel) :
                base(viewModel) {
        }
    }
}
