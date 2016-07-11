using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.Validation
{
    public interface INodeValidated
    {
        void NodeValidated(IDiagramNode node);
    }
}