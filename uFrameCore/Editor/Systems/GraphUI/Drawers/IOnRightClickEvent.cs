using uFrame.Editor.Input;

namespace uFrame.Editor.GraphUI.Drawers
{
    public interface IOnRightClickEvent
    {
        void OnRightClick(Drawer drawer, MouseEvent mouseEvent);
    }
}