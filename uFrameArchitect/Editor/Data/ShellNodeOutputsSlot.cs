using uFrame.Editor.Attributes;
using uFrame.Editor.Graphs.Data;
using uFrame.Json;

namespace uFrame.Architect.Editor.Data
{
    public class ShellNodeOutputsSlot : GenericReferenceItem<ShellSlotTypeNode>, IShellNodeItem
    {
        [JsonProperty, InspectorProperty]
        public int Order { get; set; }

        public string ReferenceClassName
        {
            get { return SourceItem.ReferenceClassName; }
        }
    }
}