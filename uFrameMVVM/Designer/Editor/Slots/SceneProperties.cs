using uFrame.Editor.Graphs.Data;

namespace uFrame.MVVM
{
    public class SceneProperties : ScenePropertiesBase
    {
        public override string Name
        {
            get { return "Scene Properties"; }
            set { base.Name = value; }
        }
    }

    public partial interface IScenePropertiesConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
