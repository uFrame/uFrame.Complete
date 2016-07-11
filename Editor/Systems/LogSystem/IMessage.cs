namespace uFrame.Editor.LogSystem
{
    public interface IMessage
    {
        MessageType MessageType { get; set; }
        string Message { get; set; }
    }
}