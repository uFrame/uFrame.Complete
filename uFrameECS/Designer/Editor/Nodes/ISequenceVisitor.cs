using uFrame.Editor.Graphs.Data;

namespace uFrame.ECS.Editor
{
    public interface ISequenceVisitor
    {
        void Visit(IDiagramNodeItem item);
        
    }
}