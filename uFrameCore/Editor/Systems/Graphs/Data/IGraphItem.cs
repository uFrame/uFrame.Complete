using uFrame.Editor.Core;
using uFrame.Editor.Database.Data;

namespace uFrame.Editor.Graphs.Data
{
    public interface IGraphItem : IItem, IDataRecord
    {
        string Label { get; }
        bool IsValid { get; }


    }

     
}