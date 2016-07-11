using uFrame.Attributes;
using System;
using System.Collections.Generic;
using uFrame.Editor.Attributes;
using uFrame.Editor.Graphs.Data;
using uFrame.Json;

namespace uFrame.ECS.Editor
{


    public interface IDescriptorItem : IDiagramNodeItem, ITypedItem
    {
        IEnumerable<DescriptorNode> Descriptors { get; }
        
    }
    public class PropertiesChildItem : PropertiesChildItemBase, IDescriptorItem
    {
        private string _friendlyName;


        public override string OutputDescription
        {
            get { return "Connect to any other node which represents a Type. This will set corresponding type of the property."; }
        }
   
        public override string RelatedTypeName
        {
            get
            {
                if (Type == uFrameECS.EntityComponentType)
                {
                    return typeof (int).Name;
                }
                return base.RelatedTypeName;
            }
        }

        public override Type Type
        {
            get
            {
                return base.Type ?? typeof(int);
            }
        }

        [JsonProperty]
        public string FriendlyName
        {
            get
            {
                if (string.IsNullOrEmpty(_friendlyName))
                    return Name;
                return _friendlyName;
            }
            set { _friendlyName = value; }
        }


        public override string DefaultTypeName
        {
            get { return typeof(int).Name; }
        }

        [InspectorProperty, NodeFlag("Mapping", NodeColor.Blue)]
        public bool Mapping
        {
            get
            {

                return this["Mapping"] || this.Type == uFrameECS.EntityComponentType;
            }
            set { this["Mapping"] = value; }
        }

        public override IEnumerable<IFlagItem> DisplayedFlags
        {
            get
            {
                foreach (var item in Descriptors)
                    yield return item;
            }
        }

        [InspectorProperty]
        public bool HideInUnityInspector
        {
            get { return this["HideInUnityInspector"]; }
            set { this["HideInUnityInspector"] = value; }
        }



        public override IEnumerable<Attribute> GetAttributes()
        {
            if (Mapping)
            {
                yield return new uFrameEventMapping(this.Name);
            }
          

        }

        public IEnumerable<DescriptorNode> Descriptors
        {
            get
            {
                foreach (var item in this.Repository.All<DescriptorNode>())
                {
                    if (this[item.Identifier])
                    {
                        yield return item;
                    }
                }
            }
        }
    }
    
    public partial interface IPropertiesConnectable : IDiagramNodeItem, IConnectable {
    }
}
