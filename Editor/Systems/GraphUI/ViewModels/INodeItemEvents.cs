using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.GraphUI.ViewModels
{
    public interface INodeItemEvents
    {
        void Renamed(IDiagramNodeItem nodeItem, string editText, string name);
    }
}