using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.Nodes
{
    public interface IGraphItemEvents
    {
        void GraphItemCreated(IGraphItem node);
        void GraphItemRemoved(IGraphItem node);
        void GraphItemRenamed(IGraphItem node);
    }
}