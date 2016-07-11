namespace uFrame.Editor.Core
{
    public interface IExecuteCommand<in TCommandType> where TCommandType : ICommand
    {
        void Execute(TCommandType command);
    }
}