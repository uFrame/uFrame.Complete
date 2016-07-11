using uFrame.Editor.Attributes;
using System.Linq;
using uFrame.Editor.Graphs.Data;

namespace uFrame.ECS.Editor
{
    public class AnyFalseNode : AnyFalseNodeBase {
        public override string GetExpression()
        {
            return "";
            //return "(" + string.Join(" || ", ExpressionsInputSlot.Items.OfType<BoolExpressionNode>().Select(p => "!" + p.GetExpression()).ToArray()) + ")";
        }
    }
    
    public partial interface IAnyFalseConnectable : IDiagramNodeItem, IConnectable {
    }
}
