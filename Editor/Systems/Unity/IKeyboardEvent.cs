using uFrame.Editor.Input;
using UnityEngine;

namespace uFrame.Editor.Unity
{
    public interface IKeyboardEvent
    {
        bool KeyEvent(KeyCode keyCode, ModifierKeyState state);
    }
}