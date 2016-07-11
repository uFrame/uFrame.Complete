using uFrame.Editor.Core;
using UnityEngine;

namespace uFrame.Editor.NavigationSystem
{
    public class NavigateCommand : Command
    {
        public string FilterId { get; set; }
        public string ItemId { get; set; }
        public string GraphId { get; set; }
        public string Workspaceid { get; set; }
        public Vector2 Scroll { get; set; }
    }
}