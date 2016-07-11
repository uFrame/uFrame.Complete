using System.ComponentModel;
using uFrame.Editor.Core;
using uFrame.Editor.Koinonia.Data;

namespace uFrame.Editor.Koinonia.Commands
{
    public class QueueRevisionForInstallCommand : IBackgroundCommand
    {
        public BackgroundWorker Worker { get; set; }
        public UFramePackageDescriptor PackageDescriptor { get; set; }
        public UFramePackageRevisionDescriptor RevisionDescriptor { get; set; }
        public string Title { get; set; }
    }
}
