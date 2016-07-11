using System.CodeDom;
using uFrame.Attributes;
using uFrame.ECS.Editor;
using uFrame.Editor.Database.Data;
using uFrame.Editor.DebugSystem;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Nodes;
using uFrame.Json;
using uFrame.Editor.Attributes;
using uFrame.Editor.Compiling.CodeGen;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    

    public class CustomActionNode : CustomActionNodeBase, IActionMetaInfo, IDemoVersionLimit, IClassNode
    {
        public override IEnumerable<IContextVariable> GetContextVariables()
        {
            yield return new ContextVariable("this")
            {

                Node = this,
                VariableType = this,
                Repository = this.Repository,
            };
            foreach (var item in this.Inputs)
            {
                yield return new ContextVariable(item.Name)
                {
                    VariableType = item.MemberType,
                    Node = this,
                    Repository = Repository
                };
            }
            //return base.GetContextVariables();
        }
        public override bool AllowOutputs
        {
            get { return false; }
        }
        public override bool AllowInputs
        {
            get { return false; }
        }
    
        private uFrameCategory _category;
        private bool _isAsync;
        private ActionDescription _descriptionAttribute;
        private bool _codeAction;

        [JsonProperty,InspectorProperty]
        public string ActionTitle { get; set; }

        public IEnumerable<string> CategoryPath
        {
            get { yield return this.Graph.Name; }
        }

        public bool IsEditorClass
        {
            get { return false; }
            set { }
        }

        public uFrameCategory Category
        {
            get { return _category ?? (_category = new uFrameCategory(Graph.Name)); }
            set { _category = value; }
        }

        public virtual ActionDescription DescriptionAttribute
        {
            get { return _descriptionAttribute ?? (_descriptionAttribute = new ActionDescription("")); }
            set { _descriptionAttribute = value; }
        }

        [InspectorProperty, JsonProperty]
        public bool CodeAction
        {
            get { return !Children.Any(); }
            set {  }
        }

        [InspectorProperty, JsonProperty]
        public bool IsAsync
        {
            get { return _isAsync; }
            set { this.Changed("IsAsync", ref _isAsync, value); }
        }

        public void WriteCode(TemplateContext ctx, ActionNode node)
        {
            //base.WriteCode();
            var varStatement = ctx.CurrentDeclaration._private_(node.Meta.FullName, node.VarName);
            varStatement.InitExpression = new CodeObjectCreateExpression(node.Meta.FullName);

            foreach (var item in node.GraphItems.OfType<IActionIn>())
            {
                var contextVariable = item.Item;
                if (contextVariable == null)
                    continue;
                ctx._("{0}.{1} = {2}", varStatement.Name, item.Name, item.VariableName);
            }


            ctx._("{0}.System = System", varStatement.Name);


            foreach (var item in node.GraphItems.OfType<ActionBranch>())
            {
                var branchOutput = item.OutputTo<SequenceItemNode>();
                if (branchOutput == null) continue;
                if (DebugSystem.IsDebugMode)
                    ctx._("{0}.{1} = ()=> {{ System.StartCoroutine({2}()); }}", varStatement.Name, item.Name, item.VariableName);
                else
                    ctx._("{0}.{1} = {2}", varStatement.Name, item.Name, item.VariableName);
            }

            ctx._("{0}.Execute()", varStatement.Name);

            node.WriteActionOutputs(ctx);
        }

        public override void Accept(ISequenceVisitor csharpVisitor)
        {
           
            base.Accept(csharpVisitor);
            csharpVisitor.Visit(this);
        }

        public override void WriteCode(ISequenceVisitor visitor, TemplateContext ctx)
        {
            base.WriteCode(visitor, ctx);
            foreach (var item in Outputs)
            {
                var v = item.InputFrom<IContextVariable>();
                if (v != null)
                ctx._("{0} = {1}", item.Name, v.VariableName);
            }

        }

        public override void WriteActionOutputs(TemplateContext _)
        {
            base.WriteActionOutputs(_);
            foreach (var item in Outputs)
            {
                var v = item.InputFrom<IContextVariable>();
                _._("{0} = {1}",item.Name, v.VariableName);
            }


        }
    }
    
    public partial interface ICustomActionConnectable : IDiagramNodeItem, IConnectable {
    }
}
