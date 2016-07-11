using System.Linq;
using System.Reflection;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Architect.Editor.Data
{
    public class TemplateReference : GenericNodeChildItem
    {
        public ShellGeneratorTypeNode GeneratorNode
        {
            get { return this.Node as ShellGeneratorTypeNode; }
        }

        public MemberInfo MemberInfo
        {
            get { return GeneratorNode.TemplateMembers.FirstOrDefault(p => p.Name == this.Name); }
        }

        public IShellNodeItem SelectorItem
        {
            get
            {
                return this.InputFrom<IShellNodeItem>();
            }
        }

        //public IShellReferenceType SelectorItemSection
        //{
        //    get
        //    {
        //        if (SelectorItem == null)
        //        {
        //            Debug.Log("SelectorItem is null ");
        //            return null;
        //        }
        //        return SelectorItem.SourceItemObject as IShellReferenceType;
        //    }
        //}

        public string SelectorItemSectionSourceType
        {
            get
            {
                if (SelectorItem == null)
                {
                    return null;
                }
                return SelectorItem.ReferenceClassName;
            }
        }


    }
}