using System;
using UnityEngine;

namespace uFrame.Editor.GraphUI
{
    public class DesignerWindowModalContent
    {
        public Action<Rect> Drawer { get; set; }
        public int ZIndex { get; set; }
    }
}