using uFrame.Editor.Graphs.Data;

namespace uFrame.ECS.Editor
{
    using System.Linq;


    public class AllTrueNode : AllTrueNodeBase {
        public override string Name
        {
            get { return "All True"; }
            set { base.Name = value; }
        }

    
        public override string GetExpression()
        {
            return "(" + string.Join(" && ", ExpressionsInputSlot.Items.OfType<BoolExpressionNode>().Select(p => p.GetExpression()).ToArray()) + ")";
        }
    }
    
    public partial interface IAllTrueConnectable : IDiagramNodeItem, IConnectable {
    }
}
