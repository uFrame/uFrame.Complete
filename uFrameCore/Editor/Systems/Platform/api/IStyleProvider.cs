using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI.Drawers.Schemas;

namespace uFrame.Editor.Platform
{
    public interface IStyleProvider
    {
        object GetImage(string name);
        object GetStyle(InvertStyles name);
        object GetFont(string fontName);

        INodeStyleSchema GetNodeStyleSchema(NodeStyle name);
        IConnectorStyleSchema GetConnectorStyleSchema(ConnectorStyle name);
        IBreadcrumbsStyleSchema GetBreadcrumbStyleSchema(BreadcrumbsStyle name);
    }
}