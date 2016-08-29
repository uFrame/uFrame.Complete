using uFrame.Editor.Workspaces;
using uFrame.Editor.Workspaces.Data;

namespace uFrame.MVVM
{
    public class MvvmWorkspace : Workspace
    {
        public override CompilationMode CompilationMode
        {
            get
            {
                return CompilationMode.Always;
            }
        }
    }
}


