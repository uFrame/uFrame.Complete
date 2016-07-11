using uFrame.Editor.GraphUI.Drawers;
using uFrame.Editor.GraphUI.ViewModels;
using uFrame.Editor.Workspaces.Data;

namespace uFrame.Editor.Platform
{
    public interface IGraphWindow
    {
        DiagramViewModel DiagramViewModel { get; }
        float Scale { get; set; }
        DiagramDrawer DiagramDrawer { get; set; }
        void RefreshContent();
        void ProjectChanged(Workspace project);
    }
}