using uFrame.Editor.Database.Data;
using UnityEngine;

namespace uFrame.Editor.Graphs.Data
{
    public interface IFilterItem : IDataRecord
    {
        bool Collapsed { get; set; }
        string NodeId { get; set; }
        string FilterId { get; set; }
        IDiagramNode Node { get; }
        IGraphFilter Filter { get; }
        Vector2 Position { get; set; }
    }
}