using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Compiling.CommonNodes;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;

namespace uFrame.MVVM
{
    public class ServiceNode : ServiceNodeBase, IClassNode
    {
        public override bool AllowInputs
        {
            get { return false; }
        }

        public override bool AllowOutputs
        {
            get { return false; }
        }

        public override IEnumerable<IItem> PossibleHandlers
        {
            get {
                return this.Graph.AllGraphItems
                    .Where(p => (p is CommandNode) || (p is SimpleClassNode) || (p is TypeReferenceNode))
                    .Cast<IItem>();
            }
        }
    }

    public partial interface IServiceConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
