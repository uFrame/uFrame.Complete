namespace uFrame.Editor.Core.MultiThreading
{
    public interface ICommandProgressEvent
    {
        void Progress(ICommand command, string message, float progress);
    }
}