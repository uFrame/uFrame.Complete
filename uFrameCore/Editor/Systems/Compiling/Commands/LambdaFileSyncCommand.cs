using System;
using uFrame.Editor.Core;

namespace uFrame.Editor.Compiling.Commands
{
    public class LambdaFileSyncCommand : LambdaCommand, IFileSyncCommand
    {
        public LambdaFileSyncCommand(string title, Action action) : base(title, action)
        {
        }
    }
}
