using uFrame.Editor.GraphUI.ViewModels;
using UnityEngine;

namespace uFrame.Editor.Windows
{
    public class WindowViewModel : GraphItemViewModel
    {
        public override Vector2 Position { get; set; }
        public override string Name { get; set; }
    }
}