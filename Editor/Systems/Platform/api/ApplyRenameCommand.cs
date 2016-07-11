using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.Platform
{
    public class ApplyRenameCommand : Command
    {
        public IDiagramNode Item { get; set; }
        public string Old { get; set; }
        public string Name { get; set; }
    }
}