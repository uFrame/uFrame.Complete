using uFrame.Editor.Workspaces.Data;

namespace uFrame.Editor.Workspaces.Events
{
    public interface IWorkspaceChanged
    {
        void WorkspaceChanged(Workspace workspace);
    }
}