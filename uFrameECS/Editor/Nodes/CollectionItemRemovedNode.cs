using System.CodeDom;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Graphs.Data;

namespace uFrame.ECS.Editor
{
    


    public class CollectionItemRemovedNode : CollectionItemRemovedNodeBase {
        public override string DisplayName
        {
            get
            {
                if (Repository != null && SourceProperty != null && SourceProperty.Source != null)
                    return string.Format("{0} Item Removed", SourceProperty.Source.MemberName);
                return "PropertyChanged";
            }
        }


        public override void WriteEventSubscription(TemplateContext ctx, CodeMemberMethod filterMethod, CodeMemberMethod handlerMethod)
        {
            //base.WriteEventSubscription(ctx, filterMethod, handlerMethod);
            var relatedTypeProperty = SourceProperty.Source.MemberType as CollectionTypeInfo;

            filterMethod.Parameters.Add(new CodeParameterDeclarationExpression(relatedTypeProperty.ChildItem.MemberType.FullName, "item"));
            handlerMethod.Parameters.Add(new CodeParameterDeclarationExpression(relatedTypeProperty.ChildItem.MemberType.FullName, "item"));

            ctx._("this.CollectionItemRemoved<{0},{1}>(Group=>{2}, {3})",
                EventType, relatedTypeProperty.ChildItem.MemberType.FullName, SourceProperty.Name, filterMethod.Name);
     
        }
        public override string HandlerMethodName
        {
            get
            {
                return Name;
                if (Repository != null && SourceProperty.Source != null)
                    return string.Format("{0}ItemAdded", SourceProperty.Source.MemberName);
                return Graph.CurrentFilter.Name + "ItemAdded";
            }
        }
        public override string HandlerFilterMethodName
        {
            get
            {
                return Name + "Filter";
                if (Repository != null && SourceProperty.Source != null)
                    return string.Format("{0}ItemAddedFilter", SourceProperty.Source.MemberName);
                return Graph.CurrentFilter.Name + "ItemAddedFilter";
            }
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
    
    public partial interface ICollectionItemRemovedConnectable : IDiagramNodeItem, IConnectable {
    }
}
