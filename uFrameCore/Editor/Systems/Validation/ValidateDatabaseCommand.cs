using System.ComponentModel;
using uFrame.Editor.Core;
using uFrame.Editor.Core.MultiThreading;
using uFrame.Editor.Database;
using uFrame.Editor.Database.Data;

namespace uFrame.Editor.Validation
{
    public class ValidateDatabaseCommand : Command, IBackgroundCommand
    {
        public IRepository Repository { get; set; }
        public BackgroundTask Task { get; set; }
        public BackgroundWorker Worker { get; set; }
        public IGraphConfiguration GraphConfiguration { get; set; }
        public string FullPath { get; set; }
    }
}