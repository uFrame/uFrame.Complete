using System.CodeDom;
using uFrame.Editor.Attributes;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;

namespace uFrame.ECS.Editor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class PropertyChangedNode : PropertyChangedNodeBase, ISequenceNode, ISetupCodeWriter {
        private PropertyIn _PropertyIn;
        private string _PropertyInId;
        private bool _immediate;
        private bool _onlyWhenChanged;

        public override bool CanGenerate { get { return true; } }
        //public override string Name
        //{
        //    get
        //    {
                
        //        return "PropertyChanged"; 
        //    }
        //    set { base.Name = value; }
        //}

        public IContextVariable SourceProperty
        {
            get { return  PropertyIn.Item; }
        }

        [Json.JsonProperty()]
        public virtual string PropertyInId
        {
            get
            {
                if (_PropertyInId == null)
                {
                    _PropertyInId = Guid.NewGuid().ToString();
                }
                return _PropertyInId;
            }
            set
            {
                _PropertyInId = value;
            }
        }
        public PropertyIn PropertyIn
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
                return _PropertyIn ?? (_PropertyIn = new PropertyIn() { Node = this, Identifier = PropertyInId, GroupIn=EntityGroup, Repository = Repository, });
            }
        }
        [Json.JsonProperty, NodeProperty("Only invoked when the property is set to a different value than the current value.")]
        public virtual bool OnlyWhenChanged
        {
            get { return _onlyWhenChanged; }
            set { this.Changed("OnlyWhenChanged", ref _onlyWhenChanged, value); }
        }

        [Json.JsonProperty, NodeProperty("Invoked immediately upon subscription.")]
        public virtual bool Immediate
        {
            get { return _immediate; }
            set { this.Changed("Immediate", ref _immediate, value); }
        }

        public override string DisplayName
        {
            get
            {
                if (Repository != null && !string.IsNullOrEmpty(this.PropertyInId) && PropertyIn != null && SourceProperty != null)
                    return string.Format("{0} Property Changed", SourceProperty.Source.MemberName);
                return "PropertyChanged";
            }
        }
        public override string HandlerMethodName
        {
            get
            {
                return this.Name;
                //if (Repository != null && !string.IsNullOrEmpty(this.PropertyInId) && PropertyIn != null && SourceProperty != null)
                //    return string.Format("{0}PropertyChanged", SourceProperty.Source.MemberName);
                //return Graph.CurrentFilter.Name + "PropertyChanged";
            }
        }
        public override string HandlerFilterMethodName
        {
            get
            {
                return this.Name + "Filter";
                if (Repository != null && !string.IsNullOrEmpty(this.PropertyInId) && PropertyIn != null && SourceProperty != null)
	                return string.Format("{0}PropertyChangedFilter", SourceProperty.Source.MemberName, SourceProperty.Source.MemberName);
                return Graph.CurrentFilter.Name + "PropertyChangedFilter";
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
                errors.AddError("Source Property not set",this.Node);
        }
        public override IEnumerable<IMemberInfo> GetMembers()
        {
            var source = SourceProperty.Source;
            if (source == null)
            {
                return base.GetMembers();
            }
            return base.GetMembers().Concat(new[]
            {
                new DefaultMemberInfo()
                {
                    MemberName = "NewValue",
                    MemberType = source.MemberType
                },
                new DefaultMemberInfo()
                {
                    MemberName = "OldValue",
                    MemberType = source.MemberType
                }
            });
        }

        public override IEnumerable<IContextVariable> GetContextVariables()
	    {
		    if (SourceProperty == null) {
		    	return base.GetContextVariables();
		    }
            var source = SourceProperty.Source;
            if (source == null)
            {
                return base.GetContextVariables();
            }
            return base.GetContextVariables().Concat(new[]
            {
                new ContextVariable("OldValue")
                {
                    Node = this,
                    VariableType =  source.MemberType,
                    Source =  source,
                    Repository = Repository
                },
                 new ContextVariable("NewValue")
                {
                    Node = this,
                    VariableType =  source.MemberType,
                    Source =  source,
                    Repository = Repository
                },
            });
        }
        public override void AddProperties(TemplateContext<HandlerNode> ctx)
        {
            base.AddProperties(ctx);
            var source = SourceProperty.Source;
            if (source == null)
            {
                return;
            }
            ctx.CurrentDeclaration._public_(source.MemberType.FullName, "OldValue");
            ctx.CurrentDeclaration._public_(source.MemberType.FullName, "NewValue");
        }

        protected override void WriteHandlerInvoker(CodeMethodInvokeExpression handlerInvoker, CodeMemberMethod handlerFilterMethod)
        {
            base.WriteHandlerInvoker(handlerInvoker, handlerFilterMethod);
            handlerInvoker.Parameters.Add(new CodeSnippetExpression("value"));
        }
        protected override void WriteHandlerSetup(TemplateContext ctx, string name, CodeMemberMethod handlerMethod)
        {
            base.WriteHandlerSetup(ctx, name, handlerMethod);
            ctx._("{0}.OldValue = value.PreviousValue", name);
            ctx._("{0}.NewValue = value.CurrentValue", name);
        }
        public override void WriteEventSubscription(TemplateContext ctx, CodeMemberMethod filterMethod, CodeMemberMethod handlerMethod)
        {
            //base.WriteEventSubscription(ctx, filterMethod, handlerMethod);
            var relatedTypeProperty = SourceProperty.Source;
	        filterMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("PropertyChangedEvent<{0}>",relatedTypeProperty.MemberType.FullName), "value"));
            handlerMethod.Parameters.Add(new CodeParameterDeclarationExpression(string.Format("PropertyChangedEvent<{0}>", relatedTypeProperty.MemberType.FullName), "value"));
            if (Immediate)
            {
                ctx._("this.PropertyChangedEvent<{0},{1}>(Group=>{2}Observable, {3}, Group=>{2}, {4})", 
                    EventType, relatedTypeProperty.MemberType.FullName, SourceProperty.Name, filterMethod.Name, OnlyWhenChanged ? "true" : "false");
            }
            else
            {
                ctx._("this.PropertyChangedEvent<{0},{1}>(Group=>{2}Observable, {3}, null, {4})", EventType, relatedTypeProperty.MemberType.FullName, SourceProperty.Name, filterMethod.Name, OnlyWhenChanged ? "true" : "false");
            }
            
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
    
    public partial interface IPropertyChangedConnectable : IDiagramNodeItem, IConnectable {
    }
}
