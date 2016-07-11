using uFrame.Editor.Core;
using UnityEngine;

namespace uFrame.Editor.NavigationSystem
{
    public class ScrollGraphCommand : Command
    {
        public Vector2 Position;

        public string Title
        {
            get { return "ScrollTo"; }
            set { }
        }
    }
}