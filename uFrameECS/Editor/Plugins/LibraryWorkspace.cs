using uFrame.Editor.Workspaces;
using uFrame.Editor.Workspaces.Data;

namespace uFrame.ECS.Editor
{
    public class LibraryWorkspace : Workspace
    {
        public override CompilationMode CompilationMode
        {
            get { return CompilationMode.Always; }
        }
    }

    public class EcsWorkspace : Workspace
    {
        public override CompilationMode CompilationMode
        {
            get { return CompilationMode.Always; }
        }
    }
}