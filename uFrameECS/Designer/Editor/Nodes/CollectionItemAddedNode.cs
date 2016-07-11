using System.CodeDom;
using uFrame.Editor.Attributes;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;

namespace uFrame.ECS.Editor
{
    


    public class CollectionItemAddedNode : CollectionItemAddedNodeBase {
        private bool _immediate;
        [Json.JsonProperty, NodeProperty("Invoked immediately on all items upon subscription.")]
        public virtual bool Immediate
        {
            get { return _immediate; }
            set { this.Changed("Immediate", ref _immediate, value); }
        }
        public override string DisplayName
        {
            get
            {
	            if (Repository != null && SourceProperty != null && SourceProperty.Source != null)
                    return string.Format("{0} Item Added", SourceProperty.Source.MemberName);
                return "ItemAdded";
            }
        }

        public override void WriteEventSubscription(TemplateContext ctx, CodeMemberMethod filterMethod, CodeMemberMethod handlerMethod)
        {
            //base.WriteEventSubscription(ctx, filterMethod, handlerMethod);
            var relatedTypeProperty = SourceProperty.Source.MemberType as CollectionTypeInfo;

            filterMethod.Parameters.Add(new CodeParameterDeclarationExpression(relatedTypeProperty.ChildItem.MemberType.FullName, "item"));
            handlerMethod.Parameters.Add(new CodeParameterDeclarationExpression(relatedTypeProperty.ChildItem.MemberType.FullName, "item"));

            

            ctx._("this.CollectionItemAdded<{0},{1}>(Group=>{2}, {3}, {4})",
                EventType, relatedTypeProperty.ChildItem.MemberType.FullName, SourceProperty.Name, filterMethod.Name, Immediate ? "true" : "false");
     
        }
        
        protected override void WriteHandlerSetup(TemplateContext ctx, string name, CodeMemberMethod handlerMethod)
        {
            base.WriteHandlerSetup(ctx, name, handlerMethod);
            ctx._("{0}.Item = item", name);
        }
        public override CodeMemberMethod WriteHandler(TemplateContext ctx)
        {
            return base.WriteHandler(ctx);

        }
    }
    
    public partial interface ICollectionItemAddedConnectable : IDiagramNodeItem, IConnectable {
    }
}
