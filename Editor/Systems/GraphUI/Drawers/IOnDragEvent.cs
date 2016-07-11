using uFrame.Editor.Input;

namespace uFrame.Editor.GraphUI.Drawers
{
    public interface IOnDragEvent
    {
        void OnDrag(Drawer drawer, MouseEvent mouseEvent);
    }
}