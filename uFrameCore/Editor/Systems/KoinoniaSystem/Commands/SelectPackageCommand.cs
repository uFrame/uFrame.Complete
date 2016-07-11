using System.ComponentModel;
using uFrame.Editor.Core;

namespace uFrame.Editor.Koinonia.Commands
{
    public class SelectPackageCommand : IBackgroundCommand
    {
        public string Id { get; set; }
        public BackgroundWorker Worker { get; set; }
        public string Title { get; set; }
    }
}
