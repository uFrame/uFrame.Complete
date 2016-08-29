using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Graphs.Data;


namespace uFrame.ECS.Editor
{
    public class SequenceVisitor : ISequenceVisitor
    {
        private List<SequenceItemNode> outputtedNodes = new List<SequenceItemNode>();

        public void Visit(IDiagramNodeItem item)
        {
            if (item == null) return;
            var handlerNode = item as ISequenceNode;
            SequenceItemNode actionNode = item as SequenceItemNode;
            var actionBranch = item as ActionBranch;
            var actionOut = item as IActionOut;
            var actionIn = item as IActionIn;
            //var setVariableNode = item as SetVariableNode;
            //var groupNode = item as ActionGroupNode;
            var handlerIn = item as HandlerIn;
            if (handlerIn != null)
            {
                BeforeVisitHandlerIn(handlerIn);
                VisitHandlerIn(handlerIn);
                AfterVisitHandlerIn(handlerIn); return;
            }

            if (actionNode != null)
            {
                BeforeVisitAction(actionNode);
                VisitAction(actionNode);
                AfterVisitAction( actionNode); return;
            }

            if (actionBranch != null)
            {
                BeforeVisitBranch(actionBranch);
                VisitBranch(actionBranch);
                AfterVisitBranch(actionBranch);
                return;
            }
                
            if (actionOut != null)
            {
                BeforeVisitOutput(actionOut);
                VisitOutput(actionOut);
                AfterVisitOutput(actionOut);
                return;
            }
                
            if (actionIn != null)
            {
                BeforeVisitInput(actionIn);
                VisitInput(actionIn);
                AfterVisitInput(actionIn);
                return;
            }
                
        }

        public virtual void AfterVisitGroup(ActionGroupNode groupNode)
        {
                
        }

        public virtual void VisitGroup(ActionGroupNode groupNode)
        {
          
        }

        public virtual void BeforeVisitGroup(ActionGroupNode groupNode)
        {
            
        }

        public virtual void AfterVisitHandlerIn(HandlerIn handlerIn)
        {
            
        }

        public virtual void VisitHandlerIn(HandlerIn handlerIn)
        {
                
        }

        public virtual void BeforeVisitHandlerIn(HandlerIn handlerIn)
        {
                
        }

        public virtual void BeforeSetVariableHandler(SetVariableNode setVariableNode)
        {
            //Visit(setVariableNode.VariableInputSlot);
            //Visit(setVariableNode.ValueInputSlot);
        }

        public virtual void VisitSetVariable(SetVariableNode setVariableNode)
        {
            
           
        }

        private void AfterVisitSetVariable(SetVariableNode setVariableNode)
        {
            Visit(setVariableNode.Right);
        }

        public virtual void AfterVisitInput(IActionIn actionIn)
        {
            
        }

        public virtual void BeforeVisitHandler(ISequenceNode handlerNode)
        {
            
        }

        public virtual void AfterVisitHandler(ISequenceNode handlerNode)
        {
            
        }


        public virtual void BeforeVisitBranch(ActionBranch actionBranch)
        {
                    
        }

        public virtual void AfterVisitBranch(ActionBranch actionBranch)
        {
                
        }

        public virtual void BeforeVisitOutput(IActionOut actionOut)
        {
                
        }

        public virtual void AfterVisitOutput(IActionOut actionIn)
        {
                
        }

        public virtual void BeforeVisitInput(IActionIn actionIn)
        {
            var actionOutput = actionIn.InputFrom<ActionOut>();
            if (actionOutput == null) return;

            var preferedIn = actionIn.Node.InputFrom<SequenceItemNode>();
            if (preferedIn == actionOutput.Node) return;

            var actionNode = actionOutput.Node as ActionNode;

            if (actionNode != null)
            {
                if (outputtedNodes.Contains(actionNode)) return;
                Visit(actionNode);
            }
        }

        public virtual void BeforeVisitAction(SequenceItemNode actionNode)
        {
            outputtedNodes.Add(actionNode);

            foreach (var input in actionNode.GraphItems.OfType<IActionIn>())
            {
                Visit(input);
            }
        }

        public virtual void AfterVisitAction(SequenceItemNode actionNode)
        {
            var hasInferredOutput = false;
            foreach (var output in actionNode.GraphItems.OfType<ActionOut>())
            {
                Visit(output);
            }
            foreach (var output in actionNode.GraphItems.OfType<ActionBranch>())
            {
                Visit(output);
                if (output.OutputTo<ActionNode>() != null)
                {
                    hasInferredOutput = true;
                }
            }
            //if (!hasInferredOutput)
            //{
                var innerRight = actionNode.OutputsTo<SequenceItemNode>().FirstOrDefault(p => p.Filter == actionNode);
                if (innerRight != null)
                    Visit(innerRight);
                var outterRight = actionNode.OutputsTo<SequenceItemNode>().FirstOrDefault(p => p.Filter != actionNode);
                if (outterRight != null)
                    Visit(outterRight);
            //}
        }

        public virtual void VisitAction(SequenceItemNode actionNode)
        {

        }

        public virtual void VisitBranch(ActionBranch output)
        {
            var item = output.OutputTo<SequenceItemNode>();
            if (item != null)
            {
                Visit(item);
            }
        }

        public virtual void VisitOutput(IActionOut output)
        {
                
        }

        public virtual void VisitInput(IActionIn input)
        {
            
        }

        public virtual void VisitSequenceContainer(ISequenceNode handlerNode)
        {
            Visit(handlerNode.StartNode);
        }
    }
}