using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers.Schemas;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;


    public class SetVariableNodeViewModel : SetVariableNodeViewModelBase {
        public override IEnumerable<string> Tags
        {
            get { yield break; }
        }

        public override INodeStyleSchema StyleSchema
        {
            get
            {
                return NormalStyleSchema;
            }
        }

        public override string Name
        {
            get { return "Set"; }
            set { base.Name = value; }
        }

        public SetVariableNodeViewModel(SetVariableNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
