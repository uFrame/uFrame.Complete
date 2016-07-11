using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.Compiling.Events
{
    public interface IOnCompilerError
    {
        void Error(ErrorInfo info);
    }
}