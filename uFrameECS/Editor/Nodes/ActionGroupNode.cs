using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Graphs.Data;
using UnityEngine;

namespace uFrame.ECS.Editor
{
    public class ActionGroupNode : ActionGroupNodeBase, IVariableContextProvider {
        public override Color Color
        {
            get { return Color.blue; }
        }

        public override void WriteCode(ISequenceVisitor visitor, TemplateContext ctx)
        {
            //base.WriteCode(visitor, ctx);

        }

    }
    
    public partial interface IActionGroupConnectable : IDiagramNodeItem, IConnectable {
    }
}
