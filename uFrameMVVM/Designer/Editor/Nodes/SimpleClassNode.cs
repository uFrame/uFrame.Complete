using uFrame.Editor.Graphs.Data;

namespace uFrame.MVVM
{
    public class SimpleClassNode : SimpleClassNodeBase, IHandlersConnectable, IClassNode
    {
        public override string ClassName
        {
            get { return this.Name; }
        }
    }

    public partial interface ISimpleClassConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
