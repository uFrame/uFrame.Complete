using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.Connections
{
    public interface IConnectionEvents
    {
        void ConnectionApplying(IGraphData graph, IConnectable output, IConnectable input);
        void ConnectionApplied(IGraphData graph, IConnectable output, IConnectable input);
    }
}