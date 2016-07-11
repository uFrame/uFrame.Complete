using uFrame.Editor.Compiling.Commands;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.Nodes
{
    public class PullNodeCommand : Command, IFileSyncCommand
    {
        public IDiagramNode[] Node { get; set; }
    }
}