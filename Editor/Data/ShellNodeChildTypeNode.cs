using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Architect.Editor.Data
{
    public class ShellNodeChildTypeNode : GenericNode
    {
        public IEnumerable<IReferenceNode> IncludedInSections
        {
            get
            {
                return Repository.AllOf<IReferenceNode>().Where(p => p.AcceptableTypes.Any(x => x.SourceItem == this));
            }
        }

    }
}