using uFrame.Editor.Core;
using UnityEngine;

namespace uFrame.Editor.NavigationSystem
{
    public class ScrollGraphCommand : Command
    {
        public Vector2 Position;

        public override string Title
        {
            get { return "ScrollTo"; }
            set { }
        }
    }
}