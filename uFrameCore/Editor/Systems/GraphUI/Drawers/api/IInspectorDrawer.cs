using uFrame.Editor.Platform;

namespace uFrame.Editor.GraphUI.Drawers
{
    public interface IInspectorDrawer : IDrawer
    {
        void DrawInspector(IPlatformDrawer platformDrawer);
    }
}