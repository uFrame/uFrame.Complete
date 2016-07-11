using uFrame.Editor.Attributes;
using uFrame.Editor.Core;
using uFrame.Editor.Workspaces.Data;

namespace uFrame.Editor.Workspaces.Commands
{
    public class ConfigureWorkspaceCommand : ICommand
    {
        public string Title { get; set; }

        [InspectorProperty]
        public string Name { get; set; }

        public Workspace Workspace { get; set; }

    }
}