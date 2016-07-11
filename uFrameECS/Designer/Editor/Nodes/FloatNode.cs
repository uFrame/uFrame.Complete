using System.CodeDom;
using uFrame.Editor.Attributes;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;
using uFrame.Json;


namespace uFrame.ECS.Editor
{    
    public class FloatNode : FloatNodeBase {
        private float _value;
        public override string Name
        {
            get { return "Float Variable"; }
            set
            {

            }
        }
        public override ITypeInfo VariableType
        {
            get { return new SystemTypeInfo(typeof(float)); }
        }

        [NodeProperty, JsonProperty]
        public float Value
        {
            get { return _value; }
            set { this.Changed("Value", ref _value, value); }
        }

        public override CodeExpression GetCreateExpression()
        {
            return new CodePrimitiveExpression(Value);
        }

        public override string ValueExpression
        {
            get { return Value.ToString(); }
        }
    }
    
    public partial interface IFloatConnectable : IDiagramNodeItem, IConnectable {
    }
}
