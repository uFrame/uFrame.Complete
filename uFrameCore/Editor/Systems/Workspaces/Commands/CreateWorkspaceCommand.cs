using System;
using uFrame.Editor.Attributes;
using uFrame.Editor.Core;
using uFrame.Editor.Workspaces.Data;

namespace uFrame.Editor.Workspaces.Commands
{
    public class CreateWorkspaceCommand : Command
    {
        [InspectorProperty]
        public string Name { get; set; }
        public Workspace Result { get; set; }
        public Type WorkspaceType { get; set; }
    }
}