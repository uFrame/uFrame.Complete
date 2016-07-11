using System.CodeDom;
using uFrame.Editor.Attributes;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;
using uFrame.Json;

namespace uFrame.ECS.Editor
{
    public class StringNode : StringNodeBase {
        private string _value;
        public override string Name
        {
            get { return "String Variable"; }
            set
            {

            }
        }
        [NodeProperty(InspectorType.TextArea), JsonProperty]
        public string Value
        {
            get { return _value; }
            set { this.Changed("Value", ref _value, value); }
        }

        public override string ValueExpression
        {
            get { return string.Format("\"{0}\"", JSONNode.Escape(Value)); }
        }

        public override ITypeInfo VariableType
        {
            get { return new SystemTypeInfo(typeof(string)); }
        }

        public override CodeExpression GetCreateExpression()
        {
            return new CodePrimitiveExpression(Value);
        }
    }
    
    public partial interface IStringConnectable : IDiagramNodeItem, IConnectable {
    }
}
