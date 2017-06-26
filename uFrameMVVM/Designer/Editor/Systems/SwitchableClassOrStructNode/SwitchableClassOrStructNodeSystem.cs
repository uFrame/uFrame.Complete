using System.Collections.Generic;
using System.Linq;
using uFrame.Editor;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI;
using uFrame.Editor.Input;
using uFrame.Editor.Platform;

namespace uFrame.MVVM
{
    public class SwitchableClassOrStructNodeSystem : DiagramPlugin
        , IExecuteCommand<SetNodeIsStructCommand>
    {
        public void Execute(SetNodeIsStructCommand command)
        {
            command.Item.IsStruct = command.IsStruct;
            GenericInheritableNode inheritableNode = command.Item as GenericInheritableNode;
            command.ItemViewModel.IsDirty = true;
            if (command.IsStruct)
            {
                if (inheritableNode != null)
                {
                    ConnectionData[] baseConnections =
                        inheritableNode.Inputs
                        .Where(connectable => inheritableNode.GetType().IsInstanceOfType(connectable.GetOutput(inheritableNode as IConnectableProvider)))
                        .ToArray();

                    if (baseConnections.Length > 0)
                    {
                        bool result =
                            InvertGraphEditor.Platform.MessageBox(
                                "Set Struct Mode",
                                "This node has base nodes, the connection will be lost.",
                                "OK",
                                "Cancel");
                        if (!result)
                            return;
                    }

                    if (inheritableNode.DerivedNodes.Any())
                    {
                        ConnectionData[] derivedConnections = inheritableNode.Outputs
                            .Where(connectable => connectable.GetInput(inheritableNode as IConnectableProvider).GetType().IsInstanceOfType(inheritableNode))
                            .ToArray();

                        if (derivedConnections.Length > 0)
                        {
                            bool result =
                                InvertGraphEditor.Platform.MessageBox(
                                    "Set Struct Mode",
                                    "This node has derived nodes, the connection will be lost.",
                                    "OK",
                                    "Cancel");
                            if (!result)
                                return;
                        }

                        foreach (ConnectionData baseConnection in derivedConnections)
                        {
                            inheritableNode.Repository.Remove(baseConnection);
                        }
                    }

                    foreach (ConnectionData baseConnection in baseConnections)
                    {
                        inheritableNode.Repository.Remove(baseConnection);
                    }
                }
            }

            InvertGraphEditor.DesignerWindow.RefreshContent();
        }
    }
}
