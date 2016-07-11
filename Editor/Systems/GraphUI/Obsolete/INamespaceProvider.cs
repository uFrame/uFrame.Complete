using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.GraphUI
{
    public interface INamespaceProvider
    {
        string GetNamespace(IDiagramNode node);
    }
}