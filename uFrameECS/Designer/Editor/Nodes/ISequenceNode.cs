using uFrame.Editor.Graphs.Data;

namespace uFrame.ECS.Editor
{
    public interface ISequenceNode : IDiagramNode, ICodeOutput
    {
        bool CanGenerate { get; }
        string HandlerMethodName { get; }
      //  IEnumerable<IFilterInput> FilterInputs { get; }
      //  string EventType { get; set; }
        void Accept(ISequenceVisitor csharpVisitor);
        //SequenceItemNode Right { get; }

        SequenceItemNode StartNode { get; set; }
    }
}