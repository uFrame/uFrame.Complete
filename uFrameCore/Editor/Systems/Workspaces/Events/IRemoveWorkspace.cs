using uFrame.Editor.Workspaces.Data;

namespace uFrame.Editor.Workspaces.Events
{
    public interface IRemoveWorkspace
    {
        void RemoveWorkspace(string name);
        void RemoveWorkspace(Workspace workspace);
    }
}