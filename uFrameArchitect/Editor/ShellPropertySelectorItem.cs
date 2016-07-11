using uFrame.Architect.Editor.Data;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Architect.Editor
{
    public class ShellPropertySelectorItem : GenericTypedChildItem, IShellNodeItem
    {
        public IShellNode SelectorFor
        {
            get { return this.RelatedNode() as IShellNode; }
        }

        public string ReferenceClassName
        {
            get
            {
                if (SelectorFor == null) return null;
                return SelectorFor.ClassName;
            }
        }
    }
}