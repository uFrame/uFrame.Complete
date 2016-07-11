namespace uFrame.MVVM
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Editor.Configurations;
    using uFrame.Editor.Core;
    using uFrame.Editor.Graphs.Data;


    public class StateNode : StateNodeBase, IStateConnectable
    {
        //public StateMachineNode stateMachine
        //{ 
        //    get
        //    {
        //        return this.Graph.AllGraphItems.OfType<StateMachineNode>()
        //                         .Where(n => n.Children.OfType<StateNode>().Contains(this))
        //                         .Select(n => n) as StateMachineNode;
        //    }
        //}

        public override bool AllowMultipleInputs
        {
            get { return true; }
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            foreach (var item in StateTransitions)
            {
                if (item.OutputTo<StateNode>() == null)
                {
                    errors.AddError("Transition is not connected to a state", item);
                }
            }
        }
    }

    public partial interface IStateConnectable : uFrame.Editor.Graphs.Data.IDiagramNodeItem, uFrame.Editor.Graphs.Data.IConnectable
    {
    }
}
