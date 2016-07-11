using System.CodeDom;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;
using uFrame.Json;

namespace uFrame.ECS.Editor
{
    using System;
    using System.Collections.Generic;
    
    using global::uFrame.Editor.Attributes;

    public class VariableNode : VariableNodeBase, IContextVariable , ITypedItem, IMemberInfo
    {

        public override void RecordRemoved(IDataRecord record)
        {
            base.RecordRemoved(record);
            var container = this.Filter;
            if (container == null || container.Identifier == record.Identifier)
            {
                Repository.Remove(this);
            }
        }
        public static int VariableCount;
        private string _variableName;

        public static string GetNewVariable(string prefix)
        {
            return string.Format("{0}{1}",prefix, VariableCount++);
        }

        public override string Name
        {
            get { return VariableType.TypeName + " Variable"; } 
            set { base.Name = value; }
        }

        public virtual ITypeInfo VariableType
        {
            get { return new SystemTypeInfo(typeof(object)); }
        }

        public virtual string ValueExpression
        {
            get { return string.Empty; }
        }

        public IEnumerable<IContextVariable> GetPropertyDescriptions()
        {yield break;
        }

        public virtual CodeVariableDeclarationStatement GetDeclerationStatement()
        {
            return new CodeVariableDeclarationStatement(VariableType.FullName.ToCodeReference(), VariableName, GetCreateExpression());
        }
        public virtual CodeMemberField GetFieldStatement()
        {
            return new CodeMemberField(VariableType.FullName.ToCodeReference(), VariableName)
            {
                InitExpression = GetCreateExpression()
            };
        }
        public virtual CodeExpression GetCreateExpression()
        {
            return null;
        }

        public string ShortName
        {
            get { return Name; }
        }

     

        public IMemberInfo Source
        {
            get { return this; }
            set
            {

            }
        }



        [JsonProperty, InspectorProperty]
        public string VariableName
        {
            get {
                return _variableName ?? (_variableName = VariableNameProvider.GetNewVariableName(this.GetType().Name)); }
            set { this.Changed("VariableName", ref _variableName, value); }
        }

        public IVariableNameProvider VariableNameProvider
        {
            get { return Graph as IVariableNameProvider; }
        }

        public string AsParameter
        {
            get { return Name; }
        }

        public bool IsSubVariable { get; set; }
        public string MemberName { get { return Name; } }

        public ITypeInfo MemberType
        {
            get
            {
                return VariableType;
            }
        }

        public IEnumerable<Attribute> GetAttributes()
        {
            yield break;
        }
    }

    public partial interface IVariableConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
