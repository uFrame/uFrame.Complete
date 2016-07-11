using System;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.GraphUI
{
    public interface ISerializeablePropertyData
    {
        string Name { get; }
        Type Type { get; }
        string RelatedTypeName { get; }
        string FieldName { get; }
        IDiagramNode TypeNode();
    }
}