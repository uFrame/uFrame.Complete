using uFrame.Editor.Attributes;
using uFrame.Editor.Graphs.Data;
using uFrame.Json;

namespace uFrame.Architect.Editor.Data
{
    public class ShellNodeSectionsSlot : GenericReferenceItem<ShellNodeTypeSection>, IShellNodeItem
    {
        [JsonProperty, InspectorProperty]
        public int Order { get; set; }


        public string ReferenceClassName
        {
            get { return SourceItem.ReferenceClassName; }
        }

        public override void NodeRemoved(IDiagramNode nodeData)
        {
            base.NodeRemoved(nodeData);
            if (SourceItem == nodeData)
            {
                Repository.Remove(this);
            }
        }
    }
}