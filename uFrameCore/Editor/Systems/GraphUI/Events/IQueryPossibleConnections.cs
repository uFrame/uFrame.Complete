using uFrame.Editor.GraphUI.ViewModels;
using uFrame.Editor.Platform;
using UnityEngine;

namespace uFrame.Editor.GraphUI.Events
{
    public interface IQueryPossibleConnections
    {
        void QueryPossibleConnections(SelectionMenu menu,DiagramViewModel diagramViewModel,
            ConnectorViewModel startConnector,
            Vector2 mousePosition);
    }
}