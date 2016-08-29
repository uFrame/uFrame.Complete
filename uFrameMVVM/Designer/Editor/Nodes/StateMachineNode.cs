namespace uFrame.MVVM
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Editor.Configurations;
    using uFrame.Editor.Core;
    using uFrame.Editor.Graphs.Data;


    public class StateMachineNode : StateMachineNodeBase
    {
        public override bool UseStraightLines
        {
            get
            {
                return true;
            }
        }

        public override bool AllowInputs
        {
            get { return true; }
        }

        public override bool AllowOutputs
        {
            get { return false; }
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            if (Name.ToLower() == "startstate")
            {
                errors.AddError("StartState is reserved", this, () =>
                {
                    Name = Graph.Name + "StartState";
                });
            }

            if (StartStateOutputSlot == null) return;

            if (StartStateOutputSlot.OutputTo<StateNode>() == null)
            {
                errors.AddError("State Machine requires a start state.", this);
            }
        }

        public IEnumerable<StateNode> States
        {
            get
            {
                return this.Children.OfType<StateNode>();
            }
        }

    }

    public partial interface IStateMachineConnectable : uFrame.Editor.Graphs.Data.IDiagramNodeItem, uFrame.Editor.Graphs.Data.IConnectable
    {
    }
}
