using uFrame.Editor.Input;

namespace uFrame.Editor.Platform
{
    public interface IShowContextMenu
    {
        void Show(MouseEvent evt, params object[] objects);
    }
}