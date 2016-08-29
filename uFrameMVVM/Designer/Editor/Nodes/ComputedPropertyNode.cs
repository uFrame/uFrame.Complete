namespace uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Editor.Attributes;
    using uFrame.Editor.Configurations;
    using uFrame.Editor.Core;
    using uFrame.Editor.Graphs.Data;
    using uFrame.Editor.Database.Data;
    using uFrame.Json;
    
    public class ComputedPropertyNode : ComputedPropertyNodeBase, ITypedItem 
    {
        private string _type;

        [JsonProperty]
        public string PropertyType
        {
            get
            {
                return string.IsNullOrEmpty(this._type) ? typeof(bool).Name : this._type;
            }
            set
            {
                this.Changed("PropertyType", ref _type, value);
            }
        }

        public override IEnumerable<IItem> PossibleSubProperties
        {
            get
            {
                return
                    InputProperties.Select(p => p.RelatedTypeNode)
                        .OfType<ElementNode>()
                        .SelectMany(p => p.AllProperties)
                        .Cast<IItem>();
            }
        }

        public IEnumerable<PropertiesChildItem> InputProperties
        {
            get { return this.InputsFrom<PropertiesChildItem>(); }
        }

        public override string RelatedType
        {
            get { return this.PropertyType; }
            set { this.PropertyType = value; }
        }

        [NodeProperty(InspectorType.TypeSelection)]
        public string Type
        {
            get
            {
                return this.RelatedTypeName;
            }
            set
            {
                this.PropertyType = value;
            }
        }

        public override string RelatedTypeName
        {
            get
            {
                if (Graph != null && this.Graph != null)
                {
                    var type = this.Graph.AllGraphItems.OfType<IClassTypeNode>().FirstOrDefault(p => p.Identifier == PropertyType) as IClassTypeNode;
                    if (type != null)
                    {
                        return type.ClassName;
                    }
                }
                return string.IsNullOrEmpty(PropertyType) ? typeof(Boolean).Name : PropertyType;
            }
            set { this.PropertyType = value; }
        }
    }
    
    public partial interface IComputedPropertyConnectable : uFrame.Editor.Graphs.Data.IDiagramNodeItem, uFrame.Editor.Graphs.Data.IConnectable {
    }
}
