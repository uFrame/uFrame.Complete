using uFrame.Editor.Input;

namespace uFrame.Editor.GraphUI.Drawers
{
    public interface IOnMouseUpEvent
    {
        void OnMouseUp(Drawer drawer, MouseEvent mouseEvent);
    }
}