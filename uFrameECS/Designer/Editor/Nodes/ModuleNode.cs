using uFrame.Editor.Attributes;
using uFrame.Editor.Graphs.Data;

namespace uFrame.ECS.Editor
{
    public class ModuleNode : ModuleNodeBase, IAlwaysGenerate {
        public override bool AllowOutputs
        {
            get { return false; }
        }
        public override bool AllowInputs
        {
            get { return false; }
        }
    
    }
    
    public partial interface IModuleConnectable : IDiagramNodeItem, IConnectable {
    }
}
