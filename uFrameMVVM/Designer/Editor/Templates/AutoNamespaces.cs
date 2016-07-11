using System;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Graphs.Data;

namespace uFrame.MVVM.Templates
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoNamespaces : TemplateAttribute 
    {
        public override int Priority
        {
            get
            {
                return -3;
            }
        }

        public override void Modify(object templateInstance, System.Reflection.MemberInfo info, TemplateContext ctx)
        {
            base.Modify(templateInstance, info, ctx);
            var obj = ctx.DataObject as IDiagramNodeItem;
            if(obj != null)
            {
                foreach(var item in obj.Graph.NodeItems)
                {
                    ctx.TryAddNamespace(item.Namespace);
                }
            }
        }
    }
}


