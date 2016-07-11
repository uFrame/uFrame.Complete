using uFrame.Editor.Graphs.Data;

namespace uFrame.MVVM
{
    public class Element : ElementBase {
        public override string Name {
            get
            {
                var node = this.Node as ViewNode;

                if (node == null) return "Element";
                var element = node.Element;
                if (element != null)
                {
                    return "Element: " + element.Name;
                }

                return "Element";
            }
            set { base.Name = value; }
        }
    }
    
    public partial interface IElementConnectable : IDiagramNodeItem, IConnectable {
    }
}
