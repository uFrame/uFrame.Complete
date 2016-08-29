using System.CodeDom;
using uFrame.Editor.Attributes;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;
using uFrame.Json;

namespace uFrame.ECS.Editor
{
    public class IntNode : IntNodeBase {
        private int _value;

        public override ITypeInfo VariableType
        {
            get { return new SystemTypeInfo(typeof(int)); }
        }

        public override string Name
        {
            get { return "Integer Variable"; }
            set
            {
                
            }
        }

        public override string ValueExpression
        {
            get { return Value.ToString(); }
        }

        [NodeProperty, JsonProperty]
        public int Value
        {
            get { return _value; }
            set { this.Changed("Value", ref _value, value); }
        }

        public override CodeExpression GetCreateExpression()
        {
            return new CodePrimitiveExpression(Value);
        }
    }
    
    public partial interface IIntConnectable : IDiagramNodeItem, IConnectable {
    }
}
