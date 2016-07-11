using System;
using System.Linq;
using uFrame.Editor;
using uFrame.Editor.Core;
using uFrame.Editor.Input;
using uFrame.Editor.Platform;
using UnityEditor;

namespace uFrame.ECS.Editor.Utilities
{
    public class UnityECSUtilitySystem : DiagramPlugin,
        IExecuteCommand<AddComponentToSelectionCommand>,
        IContextMenuQuery
    {
        public void Execute(AddComponentToSelectionCommand command)
        {
            if (command.ComponentType == null)
            {
                Signal<INotify>(
                    _ =>
                        _.Notify(
                            string.Format("Please, compile {0} before adding it to the scene.", command.ComponentName),
                            NotificationIcon.Warning));
            }
            else
            {
                if (Selection.gameObjects != null && Selection.gameObjects.Length > 0)
                {
                    foreach (var gameObject in Selection.gameObjects)
                    {
                        gameObject.AddComponent(command.ComponentType);
                    }
                }
            }
        }

        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, params object[] obj)
        {
            var cVM = obj.FirstOrDefault() as ComponentNodeViewModel;
            if (cVM == null) return;

            var component = cVM.ComponentNode;

            if (Selection.gameObjects != null && Selection.gameObjects.Length > 0)
            {

                ui.AddCommand(new ContextMenuItem()
                {
                    Title = "Add To Selection",
                    Command = new AddComponentToSelectionCommand()
                    {
                        ComponentName = component.Name,
                        ComponentType = InvertApplication.FindRuntimeType(component.FullName)
                    }
                });
            }

        }



    }


    public class AddComponentToSelectionCommand : ICommand
    {
        public Type ComponentType { get; set; }
        public string ComponentName { get; set; }

        public string Title
        {
            get { return "Add Component To Selection"; }
            set { }
        }
    }
}
