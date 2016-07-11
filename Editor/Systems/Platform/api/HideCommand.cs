using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.Platform
{
    public class HideCommand : Command
    {
        public IDiagramNode[] Node { get; set; }
        public IGraphFilter Filter { get; set; }
    }
}