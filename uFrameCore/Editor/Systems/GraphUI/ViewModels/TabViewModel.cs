using uFrame.Editor.Graphs.Data;
using UnityEngine;

namespace uFrame.Editor.GraphUI.ViewModels
{
    public class TabViewModel : ViewModel
    {
        public string Name { get; set; }
        public string[] Path { get; set; }
        public Rect Bounds { get; set; }
        public IGraphData Graph { get; set; }
    }
}