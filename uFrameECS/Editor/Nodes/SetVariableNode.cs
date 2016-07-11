using uFrame.Attributes;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;
    

    [ActionTitle("Set Variable"),uFrameCategory("Assign", "Set", "Variables")]
    public class SetVariableNode : CustomAction {


        private VariableIn _Variable;

        private ValueIn _Value;

        public override string Name
        {
            get { return "Set Variable"; }
            set { base.Name = value; }
        }
        [In]
        public virtual VariableIn VariableInputSlot
        {
            get
            {

                return GetSlot(ref _Variable, "Variable", _=>_.DoesAllowInputs = true);
            }
        }
        [In]
        public virtual ValueIn ValueInputSlot
        {
            get
            {
                return GetSlot(ref _Value, "Value", _ =>
                {
                    _.DoesAllowInputs = true;
                    _.Variable = VariableInputSlot;
                });
            }
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            if (VariableInputSlot.Item == null || ValueInputSlot.Item == null)
            {
                errors.AddError("Variable and Value must be set.", this.Node);
                return;
            }
            if (ValueInputSlot.Item.VariableType == null)
            {
                errors.AddError(string.Format("{0} doesn't have a type.", ValueInputSlot.Item.VariableName),this);
                return;
            }
            if (!ValueInputSlot.Item.VariableType.IsAssignableTo(VariableInputSlot.Item.VariableType))
            {
                errors.AddError(string.Format("Variable types {0} and {1} do not match.", VariableInputSlot.Item.VariableType.FullName, ValueInputSlot.Item.VariableType.FullName),this.Node);
            }
        }
         
        public override IEnumerable<IGraphItem> GraphItems
        {
            get
            {
                
                yield return VariableInputSlot;
                //if (VariableInputSlot.Item != null)
                //{
                    yield return ValueInputSlot;
                //}
                
            }
        }

        public override void WriteCode(ISequenceVisitor visitor, TemplateContext ctx)
        {
            
            var ctxVariable = VariableInputSlot.Item;
            if (ctxVariable == null) return;

            ctx._("{0} = ({1}){2}", ctxVariable.VariableName, ctxVariable.VariableType.FullName,
                ValueInputSlot.VariableName);
        }
    }

    public class ValueIn : VariableIn
    {
        public VariableIn Variable { get; set; }

        public override ITypeInfo VariableType
        {
            get
            {
                //if (Variable.Item == null) return new SystemTypeInfo(typeof (object));
                return Variable.VariableType;
                return base.VariableType;
            }
        }
    }
    public partial interface ISetVariableConnectable : IDiagramNodeItem, IConnectable {
    }
}
