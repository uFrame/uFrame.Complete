using System.ComponentModel;

namespace uFrame.Editor.Core.MultiThreading
{
    public class BackgroundTask
    {
        public BackgroundWorker Worker { get; set; }

        public BackgroundTask(BackgroundWorker worker)
        {
            Worker = worker;
        }

        public void Cancel()
        {
            Worker.CancelAsync();
            Worker.Dispose();
        }

        public bool IsRunning
        {
            get { return Worker.IsBusy; }
        }

    }
}