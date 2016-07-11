using uFrame.Editor.Core;
using uFrame.Editor.Workspaces.Data;

namespace uFrame.Editor.Workspaces.Commands
{
    public class OpenWorkspaceCommand : Command
    {
        public Workspace Workspace { get; set; }
    }
}