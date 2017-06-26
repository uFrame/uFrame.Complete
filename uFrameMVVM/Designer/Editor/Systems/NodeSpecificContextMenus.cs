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
            GraphItemViewModel switchableItemViewModel =
                obj
                    .OfType<DiagramNodeViewModel>()
                    .FirstOrDefault(commandVm => commandVm.GraphItemObject is ISwitchableClassOrStructNodeSystem);
            ISwitchableClassOrStructNodeSystem switchableItem = null;
            if (switchableItemViewModel != null)
            {
                switchableItem =
                    (ISwitchableClassOrStructNodeSystem) ((DiagramNodeViewModel) switchableItemViewModel).GraphItemObject;
            }
            else
            {
                switchableItemViewModel =
                    obj
                        .OfType<TypedItemViewModel>()
                        .FirstOrDefault(commandVm => commandVm.MemberInfo is ISwitchableClassOrStructNodeSystem);
                if (switchableItemViewModel != null)
                {
                    switchableItem =
                        (ISwitchableClassOrStructNodeSystem) ((TypedItemViewModel) switchableItemViewModel).MemberInfo;
                }
            }

            if (switchableItemViewModel != null && switchableItem != null)
            {

                ui.AddCommand(new ContextMenuItem
                {
                    Title = "Is Struct",
                    Checked = switchableItem.IsStruct,
                    Command = new SetNodeIsStructCommand
                    {
                        Item = switchableItem,
                        ItemViewModel = switchableItemViewModel,
                        IsStruct = !switchableItem.IsStruct
                    }
                });
            }
        }
    }
}
