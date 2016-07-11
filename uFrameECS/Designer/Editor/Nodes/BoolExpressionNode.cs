using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;

namespace uFrame.ECS.Editor
{
  
    public class BoolExpressionNode : BoolExpressionNodeBase, IGroupConnectable {
        public override bool AllowInputs
        {
            get { return false; }
        }
        
        public override bool AllowMultipleOutputs
        {
            get { return false; }
        }

        //public override bool Value
        //{
        //    get { return base.Value; }
        //    set { base.Value = value; }
        //}

        public virtual string GetExpression()
        {
            return "true";
        }

        public override string Name
        {
            get { return this.GetType().Name.Replace("Node","").PrettyLabel(); }
            set { base.Name = value; }
        }
    }
    
    public partial interface IBoolExpressionConnectable : IDiagramNodeItem, IConnectable {
    }
}
