using uFrame.Editor.Graphs.Data;

namespace uFrame.ECS.Editor
{
    using System.Linq;

    public class AnyTrueNode : AnyTrueNodeBase {
        //public override string GetExpression()
        //{
        //    return "(" + string.Join(" || ", ExpressionsInputSlot.Items.OfType<BoolExpressionNode>().Select(p => p.GetExpression()).ToArray()) + ")";
        //}
    }
    
    public partial interface IAnyTrueConnectable : IDiagramNodeItem, IConnectable {
    }
}
