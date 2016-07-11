using System.Linq;
using uFrame.Editor.Core;
using uFrame.Editor.GraphUI;
using uFrame.Editor.GraphUI.ViewModels;
using uFrame.Editor.Input;
using uFrame.Editor.Platform;
using uFrame.IOC;

namespace uFrame.Editor
{

    public class ConnectionSystem : DiagramPlugin
        ,IContextMenuQuery
    {
        public override void Initialize(UFrameContainer container)
        {
            base.Initialize(container);
        }

        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, params object[] obj)
        {
            var connector = obj.FirstOrDefault() as ConnectorViewModel;
            if (connector != null)
            {
                var connections =
                   InvertGraphEditor.CurrentDiagramViewModel.GraphItems.OfType<ConnectionViewModel>()
                       .Where(p => p.ConnectorA == connector || p.ConnectorB == connector).ToArray();

                foreach (var connection in connections)
                {
                    ConnectionViewModel connection1 = connection;
                     ui.AddCommand(new ContextMenuItem()
                        {
                            Title = string.Format("Remove {0}",connection1.Name),
                            Group="Remove",
                            Command = new LambdaCommand("Remove Connection", ()=> { connection1.Remove(connection1); })
                        });
             
                }
                
            }
        }
    }
}
