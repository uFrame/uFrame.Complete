namespace uFrame.Editor.LogSystem
{
    public interface ILogEvents
    {
        void Log(string message, MessageType type);
        void Log<T>(T message) where T : LogMessage,new();
    }
}