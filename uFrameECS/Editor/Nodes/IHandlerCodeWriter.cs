using System;
using System.CodeDom;
using uFrame.Editor.Compiling.CodeGen;

namespace uFrame.ECS.Editor
{
    public interface IHandlerCodeWriter
    {
        Type For { get; }

        void WriteFilterMethod(HandlerNode handlerNode, TemplateContext ctx, CodeMemberMethod handlerFilterMethod, CodeMethodInvokeExpression invoker);

        void WriteSetupMethod(HandlerNode handlerNode, TemplateContext ctx, CodeMemberMethod handlerMethod);
    }
}