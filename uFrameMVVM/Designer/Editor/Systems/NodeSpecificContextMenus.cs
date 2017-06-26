using System.Linq;
using uFrame.Editor;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI;
using uFrame.Editor.GraphUI.ViewModels;
using uFrame.Editor.Input;
using uFrame.Editor.Platform;

namespace uFrame.MVVM
{
    public class NodeSpecificContextMenus : DiagramPlugin
        , IContextMenuQuery
    {
        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, params object[] obj)
        {
            DiagramNodeViewModel switchableNodeViewModel =
                obj
                    .OfType<DiagramNodeViewModel>()
                    .FirstOrDefault(commandVm => commandVm.GraphItemObject is ISwitchableClassOrStructNodeSystem);

            if (switchableNodeViewModel != null)
            {
                ISwitchableClassOrStructNodeSystem switchableNode =
                    (ISwitchableClassOrStructNodeSystem) switchableNodeViewModel.GraphItemObject;
                ui.AddCommand(new ContextMenuItem
                {
                    Title = "Is Struct",
                    Checked = switchableNode.IsStruct,
                    Command = new SetNodeIsStructCommand
                    {
                        Item = switchableNode,
                        ItemViewModel = switchableNodeViewModel,
                        IsStruct = !switchableNode.IsStruct
                    }
                });
            }
        }
    }
}
