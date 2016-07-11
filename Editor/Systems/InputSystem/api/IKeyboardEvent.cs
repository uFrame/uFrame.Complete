using UnityEngine;

namespace uFrame.Editor.Input
{
    public interface IKeyboardEvent
    {
        bool KeyEvent(KeyCode keyCode, ModifierKeyState state);
    }
}