using uFrame.Editor.Graphs.Data;

namespace uFrame.MVVM
{


    public class SceneTypeNode : SceneTypeNodeBase {
        public override bool AllowInputs
        {
            get { return false; }
        }

        public override bool AllowOutputs
        {
            get { return false; }
        }
    }
    
    public partial interface ISceneTypeConnectable : IDiagramNodeItem, IConnectable {
    }
}
