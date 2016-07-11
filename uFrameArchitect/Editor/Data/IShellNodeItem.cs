using uFrame.Editor.Graphs.Data;

namespace uFrame.Architect.Editor.Data
{
    public interface IShellNodeItem : IConnectable, IDiagramNodeItem
    {
        string ReferenceClassName { get; }
    }
}