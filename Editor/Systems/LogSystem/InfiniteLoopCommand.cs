using System.ComponentModel;
using uFrame.Editor.Core;

namespace uFrame.Editor.LogSystem
{
    public class InfiniteLoopCommand : ICommand, IBackgroundCommand
    {
        public string Title { get;  set; }

        public BackgroundWorker Worker { get; set; }
    }
}