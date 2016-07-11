using System.CodeDom;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;

namespace uFrame.ECS.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    


    public class CollectionModifiedHandlerNode : CollectionModifiedHandlerNodeBase {
	    private CollectionIn _PropertyIn;
        private string _PropertyInId;
  

        private ActionBranch _added;
        private ActionBranch _removed;
        private ActionBranch _reset;
        private ActionBranch _moved;

        public override bool CanGenerate { get { return true; } }


        public IContextVariable SourceProperty
        {
            get { return CollectionIn.Item; }
        }

 
        public CollectionIn CollectionIn
        {
            get
            {
                if (Repository == null)
                {
                    return null;
                }
                if (_PropertyIn != null)
                {
                    return _PropertyIn;
                }
	            return GetSlot(ref _PropertyIn,"Collection", _=>{_.GroupIn = EntityGroup;});
            }
        }
        public CollectionTypeInfo CollectionInfo
        {
            get
            {
                if (SourceProperty.Source == null) return null;
                
                return SourceProperty.Source.MemberType as CollectionTypeInfo;
            }
        }
        public override void AddProperties(TemplateContext<HandlerNode> ctx)
        {
            base.AddProperties(ctx);
            var relatedTypeProperty = SourceProperty.Source.MemberType as CollectionTypeInfo;
            ctx.CurrentDeclaration._public_(relatedTypeProperty.ChildItem.MemberType.FullName, "Item");
        }

        public override IEnumerable<IMemberInfo> GetMembers()
        {
            if (CollectionInfo == null)
            {
                return base.GetMembers();
            }
            return base.GetMembers().Concat(new []
            {
                new DefaultMemberInfo()
                {
                    MemberName = "Item",
                    MemberType = CollectionInfo.ChildItem.MemberType
                }
            });
        }

        public override IEnumerable<IContextVariable> GetContextVariables()
        {
            if (CollectionInfo == null)
            {
                return base.GetContextVariables();
            }
            return base.GetContextVariables().Concat(new[]
            {
                new ContextVariable("Item")
                {
                    Node = this,
                    VariableType =  CollectionInfo.ChildItem.MemberType,
                    Source =  CollectionInfo.ChildItem,
                    Repository = Repository
                },
            });
        }
        public override string HandlerMethodName
        {
            get
            {
                return Name;

            }
        }
        public override string HandlerFilterMethodName
        {
            get
            {
                return Name + "Filter";

            }
        }


        public override string EventType
        {
            get
            {
                if (SourceProperty == null) return "...";
                return this.SourceProperty.Node.Name;
                //return SourceInputSlot.InputFrom<IMappingsConnectable>().Name;
            }
            set
            {

            }
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            if (SourceProperty == null)
                errors.AddError("Source Collection not set", this.Node);
        }

        protected override void WriteHandlerInvoker(CodeMethodInvokeExpression handlerInvoker, CodeMemberMethod handlerFilterMethod)
        {
            base.WriteHandlerInvoker(handlerInvoker, handlerFilterMethod);
            handlerInvoker.Parameters.Add(new CodeSnippetExpression("item"));
        }


        public override bool IsLoop
        {
            get { return false; }
        }

        public IEnumerable GetObservableProperties()
        {
            foreach (var item in FilterInputs)
            {
                foreach (var p in item.InputFrom<IMappingsConnectable>().GetObservableProperties())
                {
                    yield return p;
                }
            }
        }
    }
    
    public partial interface ICollectionModifiedHandlerConnectable : IDiagramNodeItem, IConnectable {
    }
}
