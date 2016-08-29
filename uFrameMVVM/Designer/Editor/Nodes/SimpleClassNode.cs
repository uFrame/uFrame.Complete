using uFrame.Editor.Graphs.Data;

namespace uFrame.MVVM
{
    public class SimpleClassNode : SimpleClassNodeBase, IHandlersConnectable
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
