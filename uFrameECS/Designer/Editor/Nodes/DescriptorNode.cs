using uFrame.Editor.Attributes;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;
using uFrame.Editor.Platform;
using uFrame.Json;
using UnityEngine;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;
    
    public enum DescriptorNodeType
    {
        Components,
        Properties,
    }
    public class DescriptorNode : DescriptorNodeBase, IMappingsConnectable, IFlagItem {
        private NodeColor _flagColor;
        private DescriptorNodeType _descriptorType = DescriptorNodeType.Properties;
        public IEnumerable<ComponentNode> SelectComponents { get { yield break; } }
        public string GetContextItemName(string mappingId)
        {
            return mappingId + "Item";
        }

        public string ContextTypeName
        {
            get { return "I" + Name; }
        }

        public string SystemPropertyName
        {
            get { return Name + "Manager"; }
        }
        public string EnumeratorExpression
        {
            get { return string.Format("{0}.Components", SystemPropertyName); }
        }
        [JsonProperty, NodeProperty]
        public DescriptorNodeType Type
        {
            get { return _descriptorType; }
            set { this.Changed("Type", ref _descriptorType, value); }
        }
        [JsonProperty, NodeProperty]
        public NodeColor FlagColor
        {
            get { return _flagColor; }
            set { this.Changed("FlagColor", ref _flagColor, value); }
        }

        public override Color Color
        {
            get { return CachedStyles.GetColor(_flagColor); }
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
        }

        public string MatchAndSelect(string mappingExpression)
        {
            return string.Format("{0}[{1}]", SystemPropertyName, mappingExpression);
        }

        public string DispatcherTypesExpression()
        {
            return SystemPropertyName + ".SelectTypes";
        }

        public IEnumerable<PropertiesChildItem> GetObservableProperties()
        {
            yield break;
        }

        NodeColor IFlagItem.Color
        {
            get { return FlagColor; }
        }
    }
    
    public partial interface IDescriptorConnectable : IDiagramNodeItem, IConnectable {
    }
}
