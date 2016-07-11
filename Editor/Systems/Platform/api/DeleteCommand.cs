using uFrame.Editor.Compiling.Commands;
using uFrame.Editor.Core;
using uFrame.Editor.Database.Data;

namespace uFrame.Editor.Platform
{
    public class DeleteCommand : Command, IFileSyncCommand
    {
        public IDataRecord[] Item { get; set; }
    }
}