using uFrame.Editor.GraphUI.ViewModels;
using UnityEngine;

namespace uFrame.Editor.GraphUI.Events
{
    public interface IShowConnectionMenu
    {
        void Show(DiagramViewModel diagramViewModel, ConnectorViewModel startConnector,Vector2 position);
    }
}