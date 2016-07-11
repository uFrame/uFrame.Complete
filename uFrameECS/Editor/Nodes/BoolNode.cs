using System.CodeDom;
using uFrame.Editor.Attributes;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;
using uFrame.Json;

namespace uFrame.ECS.Editor
{
    public class BoolNode : BoolNodeBase {
        private bool _value;

        [NodeProperty, JsonProperty]
        public virtual bool Value
        {
            get { return _value; }
            set { this.Changed("Value", ref _value, value); }
        }
        public override string Name
        {
            get { return "Bool Variable"; }
            set
            {

            }
        }
        public override ITypeInfo VariableType
        {
            get { return new SystemTypeInfo(typeof(bool)); }
        }

        public override string ValueExpression
        {
            get { return Value ? "true" : "false"; }
        }

        public override CodeExpression GetCreateExpression()
        {
            return new CodePrimitiveExpression(Value);
        }
    }
    
    public partial interface IBoolConnectable : IDiagramNodeItem, IConnectable {
    }
}
