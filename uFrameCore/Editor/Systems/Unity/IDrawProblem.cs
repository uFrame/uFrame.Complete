using uFrame.Editor.Core;
using UnityEngine;

namespace uFrame.Editor.Unity
{
    internal interface IDrawProblem
    {
        void DrawProblemInspector(Rect bounds, Problem problem);
    }
}