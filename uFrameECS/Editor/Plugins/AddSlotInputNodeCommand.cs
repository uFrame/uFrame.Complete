using uFrame.ECS.Editor;
using uFrame.Editor.Core;
using uFrame.Editor.GraphUI.ViewModels;
using UnityEngine;

namespace uFrame.ECS.Editor
{
    public class AddSlotInputNodeCommand : Command
    {
        public IContextVariable Variable { get; set; }
        public HandlerNode Handler { get; set; }
        public Vector2 Position { get; set; }
        public ActionIn Input { get; set; }
        public DiagramViewModel DiagramViewModel { get; set; }
    }
}