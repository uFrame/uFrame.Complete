using System.ComponentModel;

namespace uFrame.Editor.Core
{
    public interface IBackgroundCommand : ICommand
    {
        BackgroundWorker Worker { get; set; }
    }
}