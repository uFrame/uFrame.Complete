namespace uFrame.Editor.Unity
{
    public interface ITaskProgressHandler
    {
        void Progress(float progress, string message);
    }
}