using UnityEngine;

namespace uFrame.Editor.GraphUI.Events
{
    public interface IDrawUFrameWindow
    {
        void Draw(float width, float height, Vector2 scrollPosition, float scale);
    }
}