using System;
using System.Linq;
using uFrame.MVVM.StateMachines;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;
using uFrame.MVVM.ViewModels;
using System.Collections.Generic;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.DesignerFile)]
    public partial class StateMachineTemplate : StateMachine, IClassTemplate<StateMachineNode>, ITemplateCustomFilename
    {
        public TemplateContext<StateMachineNode> Ctx { get; set; }

        public string Filename
        {
            get
            {
                if (Ctx.Data.Name == null)
                {
                    throw new Exception(Ctx.Data.Name + " Graph name is empty");
                }
                return Path2.Combine(Ctx.Data.Graph.Name + "/StateMachines.designer", Ctx.Data.Name + "StateMachine.designer.cs");
            }
        }

        // Replace by ITemplateCustomFilename's Filename
        public string OutputPath { get { return ""; } }

        public bool CanGenerate { get { return true; } }


        public void TemplateSetup()
        {

        }
    }

    [RequiresNamespace("uFrame.MVVM.StateMachines")]
    public partial class StateMachineTemplate
    {
        [GenerateConstructor("vm", "propertyName")]
        public void StateMachineConstructor(ViewModel vm, string propertyName)
        {

        }

        [GenerateConstructor("null", "string.Empty")]
        public void StateMachineConstructor()
        {

        }

        [GenerateProperty]
        public override State StartState
        {
            get
            {
                Ctx._("return this.{0}", Ctx.Data.StartStateOutputSlot.OutputTo<StateNode>().Name);
                return null;
            }
        }

        public IEnumerable<TransitionsChildItem> DistinctTransitions
        {
            get { return Ctx.Data.Transitions.Distinct(); }
        }

        public IEnumerable<StateNode> DistinctStates
        {
            get { return Ctx.Data.States.Distinct(); }
        }

        [ForEach("DistinctTransitions"), GenerateProperty, WithField]
        public virtual StateMachineTrigger _TriggerName_
        {
            get
            {
                Ctx._if("this.{0} == null", Ctx.Item.Name.AsField())
                   .TrueStatements
                   ._("this.{0} = new StateMachineTrigger(this , \"{1}\")", Ctx.Item.Name.AsField(), Ctx.Item.Name);
                return null;
            }
        }

        [ForEach("DistinctStates"), GenerateProperty, WithField]
        public virtual State _StateName_
        {
            get
            {
                Ctx.SetType(Ctx.Item.Name);
                Ctx._if("this.{0} == null", Ctx.Item.Name.AsField())
                   .TrueStatements
                   ._("this.{0} = new {1}()", Ctx.Item.Name.AsField(), Ctx.Item.Name);
                return null;
            }
        }

        [GenerateMethod]
        public override void Compose(List<State> states)
        {
            //base.Compose(states);
            foreach (var state in Ctx.Data.States)
            {
                foreach (var transition in state.StateTransitions)
                {
                    var to = transition.OutputTo<StateNode>();
                    if (to == null) continue;

                    Ctx._("{0}.{1} = new StateTransition(\"{1}\", {0}, {2})", state.Name, transition.Name, to.Name);
                    Ctx._("Transitions.Add({0}.{1})", state.Name, transition.Name);
                }
                foreach (var transition in state.StateTransitions)
                {
                    //var to = transition.OutputTo<StateNode>();
                    Ctx._("{0}.AddTrigger({1}, {0}.{1})", state.Name, transition.Name);
                }
                Ctx._("{0}.StateMachine = this", state.Name);
                Ctx._("states.Add({0})", state.Name);
            }
        }
    }
}

