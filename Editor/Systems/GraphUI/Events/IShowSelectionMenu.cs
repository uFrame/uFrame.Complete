using uFrame.Editor.Platform;
using UnityEngine;

namespace uFrame.Editor.GraphUI.Events
{
    public interface IShowSelectionMenu
    {
        void ShowSelectionMenu(SelectionMenu menu, Vector2? position = null, bool useWindow = false);
    }
}