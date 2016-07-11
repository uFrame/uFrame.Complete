using uFrame.Editor.Input;

namespace uFrame.Editor.GraphUI.Drawers
{
    public interface IOnMouseDownEvent
    {
        void OnMouseDown(Drawer drawer, MouseEvent mouseEvent);
    }
}