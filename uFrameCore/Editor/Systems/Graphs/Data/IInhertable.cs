using System.Collections.Generic;
using System.ComponentModel;

namespace uFrame.Editor.Graphs.Data
{
    public interface IInhertable : IDiagramNode
    {
        [Browsable(false)]
        GenericInheritableNode BaseNode { get; }
        [Browsable(false)]
        IEnumerable<GenericInheritableNode> BaseNodes { get; }
        [Browsable(false)]
        IEnumerable<GenericInheritableNode> DerivedNodes { get; }
    }
}