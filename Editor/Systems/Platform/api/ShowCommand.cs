using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;
using UnityEngine;

namespace uFrame.Editor.Platform
{
    public class ShowCommand : Command
    {
        public IDiagramNode Node { get; set; }
        public IGraphFilter Filter { get; set; }
        public Vector2 Position { get; set; }
    }
}