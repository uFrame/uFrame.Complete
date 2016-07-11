using System.Collections.Generic;
using uFrame.Editor.Attributes;
using uFrame.Editor.Graphs.Data;

namespace uFrame.ECS.Editor
{
    public class CodeActionNode : CodeActionNodeBase {
        [InspectorProperty]
        public override string MetaType
        {
            get { return FullName; }
            set { base.MetaType = value; }
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            //base.Validate(errors);
        }
    }
    
    public partial interface ICodeActionConnectable : IDiagramNodeItem, IConnectable {
    }
}
