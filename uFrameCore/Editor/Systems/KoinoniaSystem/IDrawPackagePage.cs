using uFrame.Editor.Koinonia.Data;
using UnityEngine;

namespace uFrame.Editor.Koinonia
{
    public interface IDrawPackagePage
    {
        void DrawPackagePage(Rect bounds, UFramePackageDescriptor package);
    }
}