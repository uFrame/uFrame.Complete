using uFrame.Attributes;
using uFrame.ECS;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;


    public class OutputsChildItem : OutputsChildItemBase, IActionFieldInfo, IActionIn
    {
        public bool IsGenericArgument { get { return false; } }
        public bool IsReturn { get { return false; } }
        public bool IsByRef { get { return false; } }
        public FieldDisplayTypeAttribute DisplayType { get { return new Out(MemberName); } }
        public bool IsBranch { get { return false; } }

        public bool IsOptional
        {
            get { return false; }
        }

        public IEnumerable<IMemberInfo> DelegateMembers { get { yield break; } }
        public bool IsDelegateMember { get; set; }

        public IActionFieldInfo ActionFieldInfo { get; set; }

        public string VariableName
        {
            get { return this.Name; }
        }

        public ITypeInfo VariableType
        {
            get { return MemberType; }
        }
        public IContextVariable Item { get { return null; } }

    }

    public partial interface IOutputsConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
