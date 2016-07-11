using uFrame.Editor.Attributes;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;
using uFrame.Editor.Nodes;
using uFrame.Json;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    


    public class GroupNode : GroupNodeBase,
        IRequireConnectable, 
        IMappingsConnectable, 
        IHandlerConnectable, 
        IVariableContextProvider,
        IVariableNameProvider,
        IDemoVersionLimit,
        IComponentId
    {
        private int _componentId;

        [JsonProperty, InspectorProperty]
        public int ComponentId
        {
            get
            {
                if (_componentId == 0)
                {
                    _componentId = Repository.GetSingleLazy<ComponentIds>().NextId;
                }
                return _componentId;
            }
            set { this.Changed("ComponentId", ref _componentId, value); }
        }

        public override ITypeInfo BaseTypeInfo
        {
            get { return (SystemTypeInfo)uFrameECS.EcsGroupType; }
        }

        public override bool AllowOutputs
        {
            get { return false; }
        }

        public override bool AllowMultipleInputs
        {
            get { return false; }
        }

        public void WriteCode(TemplateContext ctx)
        {
            
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            if (!SelectComponents.Any())
            {
                errors.AddError(string.Format("No components selected for {0} group!", this.Name),this);
            }
        }

        public IEnumerable<ComponentNode> SelectComponents { get { return Require.Select(p => p.SourceItem).OfType<ComponentNode>(); } }


        public string GetContextItemName(string mappingId)
        {
            return mappingId + "Item";
        }

        public string ContextTypeName
        {
            get { return Name; }
        }

        public string SystemPropertyName
        {
            get { return Name + "Manager"; }
        }

        public string EnumeratorExpression
        {
            get { return string.Format("{0}.Components", SystemPropertyName); }
        }

        public override IEnumerable<IMemberInfo> GetMembers()
        {
  
            foreach (var item in SelectComponents)
            {
                var info = item as ITypeInfo;
                if (info != null)
                yield return new DefaultMemberInfo()
                {
                    MemberName = item.Name,
                    MemberType = item
                };
            }
        
        }

        public IEnumerable<IContextVariable> GetVariables(IFilterInput input)
        {
            yield return new ContextVariable(input.HandlerPropertyName, "EntityId")
            {
             
                Node = this,
                VariableType = new SystemTypeInfo(typeof(int)),
                Repository = this.Repository,
            };
            yield return new ContextVariable(input.HandlerPropertyName, "Entity")
            {
            
                Node = this,
                VariableType = new SystemTypeInfo(uFrameECS.EntityComponentType),
                Repository = this.Repository,
                //TypeInfo = typeof(MonoBehaviour)
            };

            foreach (var select in GetMembers())
            {

                yield return new ContextVariable(input.HandlerPropertyName, select.MemberName)
                {
                   
                    Node = this, 
                    VariableType = select.MemberType,
                    Repository = this.Repository, 
                };
                if (!select.MemberType.IsEnum)
                {
                    foreach (var item in select.MemberType.GetMembers())
                    {
                        yield return new ContextVariable(input.HandlerPropertyName, select.MemberName, item.MemberName)
                        {
                        
                            Node = this,
                            Source = item ,
                            VariableType = item.MemberType,
                            Repository = this.Repository,
                        };
                    }
                }
          
                ////yield return new ContextVariable(input.HandlerPropertyName, select.Name, "EntityId") { Repository = this.Repository, Node = this, VariableType = "int" };
                ////yield return new ContextVariable(input.HandlerPropertyName, select.Name, "Entity") { Repository = this.Repository, Node = this, VariableType = "uFrame.ECS.Entity" };

                ////foreach (var item in select.PersistedItems.OfType<ITypedItem>())
                ////{
                ////    yield return new ContextVariable(input.HandlerPropertyName, select.Name, item.Name)
                ////    {
                ////        Repository = this.Repository,
                ////        Source = item,
                ////        VariableType = item.RelatedTypeName,
                ////        Node = this
                ////    };
                ////}
            }
        }

        public string MatchAndSelect(string mappingExpression)
        {
            return string.Format("{0}[{1}]",SystemPropertyName,mappingExpression);
        }

        public string DispatcherTypesExpression()
        {
            return SystemPropertyName + ".SelectTypes";
        }

        public IEnumerable<PropertiesChildItem> GetObservableProperties()
        {
            foreach (var item in SelectComponents)
            {
                foreach (var p in item.GetObservableProperties())
                {
                    yield return p;
                }
            }
        }

        public IEnumerable<IContextVariable> GetAllContextVariables()
        {
            return GetContextVariables();
        }

        public BoolExpressionNode ExpressionNode
        {
            get { return this.InputFrom<BoolExpressionNode>(); }
        }

        public IEnumerable<PropertiesChildItem> Observables
        {
            get
            {
                foreach (var item in FilterNodes.OfType<PropertyNode>())
                {
                    var variable = item.PropertySelection.Item;
                    if (variable != null && variable.Source != null)
                    {
                        var s = variable.Source as PropertiesChildItem;
                        if (s != null)
                            yield return s;
                    }
                }
            }
        }
        public IEnumerable<IContextVariable> GetContextVariables()
        {
            foreach (var item in Require)
            {
                yield return new ContextVariable(item.Name)
                {
                    Repository = this.Repository,
                    Node = this,
                    VariableType = item.SourceItem as ITypeInfo,
	                Source = item.SourceItem as IMemberInfo,
                    Identifier = this.Identifier + ":" + this.Name
                };
            }
            yield break;
        }

        public IVariableContextProvider Left
        {
            get { return null; }
        }

        private int _variableCount;


        [JsonProperty]
        public int VariableCount
        {
            get { return _variableCount; }
            set { this.Changed("VariableCount", ref _variableCount, value); }
        }

        public string GetNewVariableName(string prefix)
        {
            return string.Format("{0}{1}", prefix, VariableCount++);
        }
    }
    
    public partial interface IContextConnectable : IDiagramNodeItem, IConnectable {
    }
}
