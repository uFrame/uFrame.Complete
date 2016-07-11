using System.ComponentModel;
using uFrame.Editor.Core;

namespace uFrame.Editor.Koinonia.Commands
{
    public class RunQueuedOperationsCommand : IBackgroundCommand
    {
        public BackgroundWorker Worker { get; set; }
        public string Title { get; set; }
    }
}
