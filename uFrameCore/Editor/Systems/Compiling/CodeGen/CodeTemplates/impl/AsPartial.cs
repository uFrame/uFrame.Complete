using System;
using System.Reflection;

namespace uFrame.Editor.Compiling.CodeGen
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AsPartial : TemplateAttribute
    {
        public override void Modify(object templateInstance, MemberInfo info, TemplateContext ctx)
        {
            base.Modify(templateInstance, info, ctx);
            ctx.CurrentDeclaration.IsPartial = true;
        }
    }
}