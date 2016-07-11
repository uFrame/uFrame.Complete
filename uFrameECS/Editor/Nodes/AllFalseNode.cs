using uFrame.Editor.Graphs.Data;

namespace uFrame.ECS.Editor
{
    using System.Linq;


    public class AllFalseNode : AllFalseNodeBase {
        public override string ToString()
        {
            return "(" + string.Join(" && ", ExpressionsInputSlot.Items.OfType<BoolExpressionNode>().Select(p => "!" + p.GetExpression()).ToArray()) + ")";
        }
    }
    
    public partial interface IAllFalseConnectable : IDiagramNodeItem, IConnectable {
    }
}
