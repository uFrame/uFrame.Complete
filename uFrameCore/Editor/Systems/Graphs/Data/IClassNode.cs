using uFrame.Editor.Graphs.Data.Types;

namespace uFrame.Editor.Graphs.Data
{
    /// <summary>
    /// Tag this interface so that it is validated with all other class nodes, ensuring the name doesn't conflict
    /// </summary>
    public interface IClassNode : IDiagramNodeItem, ITypeInfo
    {
        
    }
}