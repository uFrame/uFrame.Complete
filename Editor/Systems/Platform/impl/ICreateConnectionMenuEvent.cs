using uFrame.Editor.GraphUI;
using uFrame.Editor.GraphUI.ViewModels;
using uFrame.Editor.Input;

namespace uFrame.Editor.Platform
{
    public interface ICreateConnectionMenuEvent
    {
        void CreateConnectionMenu(ConnectionHandler viewModel, DiagramViewModel diagramViewModel, MouseEvent mouseEvent);
    }
}