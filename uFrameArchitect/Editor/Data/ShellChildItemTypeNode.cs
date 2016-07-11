using System.Collections.Generic;
using System.Linq;

namespace uFrame.Architect.Editor.Data
{
    public class ShellChildItemTypeNode : ShellInheritableNode, IShellNode
    {
        public IShellNode ReferenceType
        {
            get { return GetConnectionReference<ReferenceItemType>().InputFrom<IShellNode>(); }
        }

        public override string ClassName
        {
            get { return this.Name + "ChildItem"; }
        }
        public IEnumerable<IReferenceNode> IncludedInSections
        {
            get
            {
                return Repository.AllOf<IReferenceNode>().Where(p => p.AcceptableTypes.Any(x => x.SourceItem == this));
            }
        }



        public string ReferenceClassName
        {
            get { return "I" + Name + "Connectable"; }
        }

    }
}