using System.CodeDom;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Graphs.Data;

namespace uFrame.ECS.Editor
{
    public class UserMethodNode : UserMethodNodeBase {

        public override void WriteCode(ISequenceVisitor visitor, TemplateContext ctx)
        {
            //base.WriteCode(ctx);
            var handlerMethod = ctx.CurrentDeclaration.protected_virtual_func(typeof(void), Name, Name.ToLower());
            var handlerInvoke = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), Name);

            foreach (CodeParameterDeclarationExpression item in ctx.CurrentMethod.Parameters)
            {
                handlerMethod.Parameters.Add(item);
                handlerInvoke.Parameters.Add(new CodeVariableReferenceExpression(item.Name));
            }

            //foreach (var item in AllContextVariables)
            //{
            //    if (item.IsSubVariable) continue;
            //    var type = item.SourceVariable == null ? item.Name : item.SourceVariable.RelatedTypeName;
                
                
            //    handlerMethod.Parameters.Add(new CodeParameterDeclarationExpression(
            //       type , item.AsParameter));
            //    handlerInvoke.Parameters.Add(new CodeVariableReferenceExpression(item.ToString()));
            //}
            ctx.CurrentStatements.Add(handlerInvoke);
            //ctx.PushStatements(handlerMethod.Statements);

            //ctx.PopStatements();
        }
    }
    
    public partial interface IUserMethodConnectable : IDiagramNodeItem, IConnectable {
    }
}
