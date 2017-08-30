using System;

namespace uFrame.Editor.Graphs.Data
{
    public interface IClassTypeNode : IDiagramNodeItem
    {
        string ClassName { get; }
    }
}