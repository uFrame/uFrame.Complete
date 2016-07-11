using System;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Input;
using UnityEngine;

namespace uFrame.Editor.Platform
{
    public class CreateNodeCommand : Command
    {
        public Type NodeType { get; set; }
        public MouseEvent LastMouseEvent { get; set; }
        public IGraphData GraphData { get; set; }
        public Vector2 Position { get; set; }
    }
}