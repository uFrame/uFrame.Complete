using System.ComponentModel;
using uFrame.Editor.Core;
using uFrame.Editor.Koinonia.Classes;

namespace uFrame.Editor.Koinonia.Commands
{
    public class QueueRevisionForUninstallCommand : IBackgroundCommand
    {
        public BackgroundWorker Worker { get; set; }
        public UFramePackage Package { get; set; }
        public string Title { get; set; }
    }

}
