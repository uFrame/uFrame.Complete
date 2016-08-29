using System.CodeDom;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Graphs.Data;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;
   
    public class ComponentDestroyedNode : ComponentDestroyedNodeBase {
        public override int SetupOrder
        {
            get { return 1; }
        }

        public override string DisplayName
        {
            get
            {
                if (Repository != null && EntityGroup != null && EntityGroup.Item != null)
                    return string.Format("{0} Component Destroyed", EntityGroup.Item.Name);
                return "Component Destroyed";
            }
        }
        public override string HandlerMethodName
        {
            get
            {
                if (Repository != null && EntityGroup != null && EntityGroup.Item != null)
                    return string.Format("{0}ComponentDestroyed", EntityGroup.Item.Name);
                return "ComponentDestroyed";
            }
        }
        public override string HandlerFilterMethodName
        {
            get
            {
                if (Repository != null && EntityGroup != null && EntityGroup.Item != null)
                    return string.Format("{0}ComponentDestroyedFilter", EntityGroup.Item.Name);
                return "ComponentDestroyedFilter";
            }
        }

        public override string EventType
        {
            get
            {
                if (EntityGroup.Item == null) return "...";
                return EntityGroup.Item.Name;
                //return SourceInputSlot.InputFrom<IMappingsConnectable>().Name;
            }
            set
            {

            }
        }
        public override bool IsLoop
        {
            get { return false; }
        } public override bool CanGenerate { get { return true; } }
        public override CodeMemberMethod WriteHandlerFilter(TemplateContext ctx, CodeMemberMethod handlerMethod)
        {
            return base.WriteHandlerFilter(ctx, handlerMethod);
        }

        protected override void WriteHandlerInvoker(CodeMethodInvokeExpression handlerInvoker, CodeMemberMethod handlerFilterMethod)
        {
            base.WriteHandlerInvoker(handlerInvoker, handlerFilterMethod);
        }

        public override void WriteEventSubscription(TemplateContext ctx, CodeMemberMethod filterMethod, CodeMemberMethod handlerMethod)
        {
            if (EntityGroup.Item != null)
            {
                ctx._("{0}Manager.RemovedObservable.Subscribe(_=>{1}(_,_)).DisposeWith(this)", EntityGroup.Item,
                    handlerMethod.Name);
            }
          
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            if (EntityGroup.Item == null)
            {
                errors.AddError("Group is required.", this);
            }
        }
    }
    
    public partial interface IComponentDestroyedConnectable : IDiagramNodeItem, IConnectable {
    }
}
