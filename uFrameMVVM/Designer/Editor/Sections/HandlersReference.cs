using uFrame.Editor.Graphs.Data;

namespace uFrame.MVVM
{ 
    public class HandlersReference : HandlersReferenceBase
    {
        public override bool AllowInputs
        {
            get { return false; }
        }

        public override bool AllowOutputs
        {
            get { return false; }
        }
    }
    
    public partial interface IHandlersConnectable : IDiagramNodeItem, IConnectable {
    }
}
