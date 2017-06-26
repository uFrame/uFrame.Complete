using System.Linq;
using uFrame.Editor.Attributes;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;
using uFrame.Json;

namespace uFrame.MVVM
{
    using System.Collections.Generic;

    public class CommandNode : CommandNodeBase
        , IElementConnectable
        , IClassNode
        , ISwitchableClassOrStructNodeSystem
    {
        [JsonProperty, InspectorProperty]
        public bool IsStruct { get; set; }

        public override bool AllowMultipleOutputs {
            get {
                return !IsStruct && base.AllowMultipleOutputs;
            }
        }

        public override bool CanInputFrom(IConnectable output)
        {
            if (IsStruct && GetType().IsInstanceOfType(output))
                return false;

            return base.CanInputFrom(output);
        }

        public override bool CanOutputTo(IConnectable input)
        {
            return !IsStruct && base.CanOutputTo(input);
        }

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

            if (IsStruct && DerivedNodes.Any())
            {
                errors.AddError("Struct Command cannot have derived nodes", this);
            }

            if (this.ReferenceOf<CommandsChildItem>() == null && !DerivedNodes.Any())
            {
                errors.AddError("This node must be linked to a Element Command or have derived nodes, if you want a generic command use a 'SimpleClass'.", this);
            }
        }
    }

    public partial interface ICommandConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
