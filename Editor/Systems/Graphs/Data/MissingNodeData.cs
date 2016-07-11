using System.Collections.Generic;

namespace uFrame.Editor.Graphs.Data
{
    public class MissingNodeData : GraphNode
    {


        public override void NodeItemRemoved(IDiagramNodeItem item)
        {

        }



        public override IEnumerable<IDiagramNodeItem> DisplayedItems
        {
            get { yield break; }
        }

        public override string Label { get { return string.Empty; } }

        public override string Name
        {
            get { return Label; }

        }

        public override IEnumerable<IDiagramNodeItem> PersistedItems
        {
            get { yield break; }
            set { }
        }
    }
}