using System.CodeDom;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Graphs.Data;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;
   
    public class ComponentCreatedNode : ComponentCreatedNodeBase
    {
        public override int SetupOrder
        {
            get { return 1; }
        }

        public override bool CanGenerate { get { return true; } }
        public override string DisplayName
        {
            get
            {
                if (Repository != null && EntityGroup != null && EntityGroup.Item != null)
                    return string.Format("{0} Component Created", EntityGroup.Item.Name);
                return "Component Created";
            }
        }
        public override string HandlerMethodName
        {
            get
            {
                return string.Format("{0}", Name);
            }
        }
        public override string HandlerFilterMethodName
        {
            get
            {

                return string.Format("{0}Filter", Name);

            }
        }
        public override string EventType
        {
            get
            {
                if (EntityGroup.Item == null) return "...";
                return EntityGroup.Item.ContextTypeName;
                //return SourceInputSlot.InputFrom<IMappingsConnectable>().Name;
            }
            set
            {

            }
        }
        public override bool IsLoop
        {
            get { return false; }
        }
        public override void WriteEventSubscription(TemplateContext ctx, CodeMemberMethod filterMethod, CodeMemberMethod handlerMethod)
        {
            if (EntityGroup.Item != null)
            {
                ctx._("{0}Manager.CreatedObservable.Subscribe({1}).DisposeWith(this)", EntityGroup.Item,
                    filterMethod.Name);
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

    public partial interface IComponentCreatedConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
