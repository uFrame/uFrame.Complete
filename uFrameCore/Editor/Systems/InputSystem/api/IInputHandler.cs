using uFrame.Editor.Platform;

namespace uFrame.Editor.Input
{
    public interface IInputHandler
    {
        void OnMouseDoubleClick(MouseEvent mouseEvent);
        void OnMouseDown(MouseEvent mouseEvent);
        void OnMouseMove(MouseEvent e);
        void OnMouseUp(MouseEvent mouseEvent);
        void OnRightClick(MouseEvent mouseEvent);
        void Draw(IPlatformDrawer platform, float scale);
    }
}