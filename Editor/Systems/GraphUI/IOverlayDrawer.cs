using UnityEngine;

namespace uFrame.Editor.GraphUI
{
    public interface IOverlayDrawer
    {
        void Draw(Rect bouds);
        Rect CalculateBounds(Rect diagramRect);
    }
}