using UnityEngine;

namespace uFrame.Editor.GraphUI.Events
{
    public interface IDesignerWindowEvents
    {
        void AfterDrawGraph(Rect diagramRect);

        void BeforeDrawGraph(Rect diagramRect);

        void AfterDrawDesignerWindow(Rect windowRect);

        void DrawComplete();

        void ProcessInput();
    }
}