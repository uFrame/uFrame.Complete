using uFrame.Editor.Koinonia.Classes;
using UnityEngine;

namespace uFrame.Editor.Koinonia
{
    public interface IDrawPackageControlPanel
    {
        void DrawControlPanel(Rect bounds, UFramePackage package);
    }
}