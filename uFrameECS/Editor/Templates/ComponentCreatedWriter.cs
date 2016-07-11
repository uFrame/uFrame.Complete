using System.CodeDom;
using uFrame.ECS.APIs;
using uFrame.ECS.Editor;
using uFrame.Editor.Compiling.CodeGen;

namespace uFrame.ECS.Template
{
    public class ComponentCreatedWriter : HandlerCodeWriter<ComponentCreatedEvent>
    {
        public override void WriteFilterMethod(HandlerNode handlerNode, TemplateContext ctx, CodeMemberMethod handlerFilterMethod,
            CodeMethodInvokeExpression invoker)
        {
            var component = handlerNode.InputFrom<IMappingsConnectable>();

            invoker.Parameters.Add(new CodeSnippetExpression(string.Format("data.Component as {0}", component.Name)));
            handlerFilterMethod.Statements.Add(invoker);
        }

        public override void WriteSetupMethod(HandlerNode handlerNode, TemplateContext ctx, CodeMemberMethod handlerMethod)
        {
            var component = handlerNode.InputFrom<IMappingsConnectable>();
            ctx._("this.OnEvent<ComponentCreatedEvent>().Where(x=>x.Component is {0}).Subscribe(_=>{{ {1}(_); }}).DisposeWith(this)",
                    component.Name,
                    handlerNode.HandlerFilterMethodName
                );
        }
    }
    public class ComponentDestroyedWriter : HandlerCodeWriter<ComponentDestroyedEvent>
    {
        public override void WriteFilterMethod(HandlerNode handlerNode, TemplateContext ctx, CodeMemberMethod handlerFilterMethod,
            CodeMethodInvokeExpression invoker)
        {
            var component = handlerNode.InputFrom<IMappingsConnectable>();
            invoker.Parameters.Add(new CodeSnippetExpression(string.Format("data.Component as {0}", component.Name)));
            handlerFilterMethod.Statements.Add(invoker);
        }

        public override void WriteSetupMethod(HandlerNode handlerNode, TemplateContext ctx, CodeMemberMethod handlerMethod)
        {
            var component = handlerNode.InputFrom<IMappingsConnectable>();
            ctx._("this.OnEvent<ComponentDestroyedEvent>().Where(x=>x.Component is {0}).Subscribe(_=>{{ {1}(_); }}).DisposeWith(this)",
                    component.Name,
                    handlerNode.HandlerFilterMethodName
                );
        }
    }
}