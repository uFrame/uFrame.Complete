using System;
using System.Collections.Generic;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.GraphUI
{
    [Obsolete]
    public interface IConnectionDataProvider
    {
        IEnumerable<ConnectionData> Connections { get; }
    }
}