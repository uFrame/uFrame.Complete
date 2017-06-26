using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Attributes;
using uFrame.Editor.Graphs.Data;
using uFrame.Json;

namespace uFrame.MVVM
{
    public class SimpleClassNode : SimpleClassNodeBase
        , IHandlersConnectable
        , IClassNode
        , ISwitchableClassOrStructNodeSystem
    {
        [JsonProperty]
        public bool IsStruct { get; set; }

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

        public override string ClassName
        {
            get { return this.Name; }
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);

            if (IsStruct && DerivedNodes.Any())
            {
                errors.AddError("Struct SimpleClass cannot have derived nodes", this);
            }
        }
    }

    public partial interface ISimpleClassConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
