using System;
using uFrame.Editor.Core;
using UnityEngine;

namespace uFrame.Editor.Unity
{
    public interface IDrawTreeView
    {
        void DrawTreeView(Rect bounds, TreeViewModel viewModel, Action<Vector2, IItem> itemClicked,
            Action<Vector2, IItem> itemRightClicked = null);
    }
}