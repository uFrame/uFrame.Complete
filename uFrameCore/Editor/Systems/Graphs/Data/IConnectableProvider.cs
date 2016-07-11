using System.Collections.Generic;
using uFrame.Editor.Database.Data;

namespace uFrame.Editor.Graphs.Data
{
    public interface IConnectableProvider : IDataRecord
    {
        IEnumerable<IConnectable> Connectables { get; }
    }
}