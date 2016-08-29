using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;

namespace uFrame.MVVM
{
    public class ServiceNode : ServiceNodeBase
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
            get { return this.Graph.AllGraphItems.OfType<IClassTypeNode>().Where(p => !(p is CommandNode)).Cast<IItem>(); }
        }
    }

    public partial interface IServiceConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
