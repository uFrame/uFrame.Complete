using uFrame.Editor.Graphs.Data;

namespace uFrame.MVVM
{
    using System.Collections.Generic;


    public class CommandNode : CommandNodeBase, IElementConnectable
    {
        public override string TypeName
        {
            get
            {
                return base.TypeName + "Command";
            }
        }
        public override string ClassName
        {
            get
            {
                return base.ClassName + "Command";
            }
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            if(this.ReferenceOf<CommandsChildItem>() == null)
            {
                errors.AddError("This node must be linked to a Element Command, if you want a generic command use a 'SimpleClass'.", this);
            }
        }
    }

    public partial interface ICommandConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
