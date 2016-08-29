using uFrame.ECS.Editor;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;

namespace uFrame.ECS.Editor
{
    public class GroupActionNodes : Command
    {
        public IDiagramNodeItem[] Items;
        public SequenceItemNode Node { get; set; }
    }
}