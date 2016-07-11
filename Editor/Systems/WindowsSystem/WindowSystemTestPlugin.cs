using uFrame.Editor.GraphUI.Drawers;
using uFrame.Editor.Input;
using uFrame.Editor.Platform;
using uFrame.IOC;

namespace uFrame.Editor.Windows
{

    public class WindowSystemTestPlugin : DiagramPlugin, IContextMenuQuery
    {
        public override void Initialize(UFrameContainer container)
        {
            base.Initialize(container);
            container.RegisterDrawer<HelloWorldWindowViewModel,HelloWorldWindowDrawer>();
        }


        public void QueryContextMenu(ICommandUI ui, object obj)
        {
//            var diagramNode = obj as DiagramNodeViewModel;
//            if (diagramNode != null)
//            {
//                ui.AddCommand(new OpenWinCommand());
//            }
        }

        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, params object[] obj)
        {
          //  throw new NotImplementedException();
        }
    }
}
