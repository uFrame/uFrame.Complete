using System.CodeDom;
using System.Collections;
using System.Linq;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.ECS.Systems;
using uFrame.ECS.Editor;
using uFrame.Editor.DebugSystem;

namespace uFrame.ECS.Templates
{
    public partial class HandlerTemplate
    {
        [GenerateProperty, WithField]
        public object Event
        {
            get
            {

                this.Ctx.SetType(Ctx.Data.EventType);
                return null;
            }
            set
            {

            }
        }



        [GenerateProperty, WithField]
        public EcsSystem System { get; set; }

        [TemplateSetup]
        public void SetName()
        {
            foreach (var item in Ctx.Data.FilterInputs)
            {
                var context = item.FilterNode;
                if (context == null) continue;
                CreateFilterProperty(item, context);
            }
            Ctx.Data.AddProperties(this.Ctx);

        }

        private void CreateFilterProperty(IFilterInput input, IMappingsConnectable inputFilter)
        {
            Ctx.CurrentDeclaration._public_(inputFilter.ContextTypeName, input.HandlerPropertyName);

        }

    }

    public class CSharpSequenceVisitor : SequenceVisitor
    {
        public TemplateContext _ { get; set; }

        private CodeMethodInvokeExpression _currentActionInvoker;



        public override void BeforeVisitAction(SequenceItemNode actionNode)
        {

            base.BeforeVisitAction(actionNode);


        }

        public override void VisitAction(SequenceItemNode actionNode)
        {
            _._comment(actionNode.GetType().Name);
            actionNode.OutputVariables(_);
            //actionNode.WriteActionInputs(_);
            actionNode.WriteDebugInfo(_);
            actionNode.WriteCode(this, _);
            var outputBranch = actionNode.OutputTo<BranchesChildItem>();
            if (outputBranch != null)
            {
                _._("{0}()", outputBranch.Name);
            }
            // actionNode.WriteActionOutputs(_);
        }

        public override void VisitOutput(IActionOut output)
        {
            base.VisitOutput(output);
            if (output.ActionFieldInfo != null)
                _.TryAddNamespace(output.ActionFieldInfo.MemberType.Namespace);

            if (output is ActionBranch) return;
            var varDecl = new CodeMemberField(
                output.VariableType.FullName.Replace("&", "").ToCodeReference(),
                output.VariableName
                )
            {
                InitExpression = new CodeSnippetExpression(string.Format("default( {0} )", output.VariableType.FullName.Replace("&", "")))
            };
            _.CurrentDeclaration.Members.Add(varDecl);

        }

        public override void VisitSetVariable(SetVariableNode setVariableNode)
        {
            base.VisitSetVariable(setVariableNode);

        }


        public override void VisitBranch(ActionBranch output)
        {
            var branchMethod = new CodeMemberMethod()
            {
                ReturnType = !DebugSystem.IsDebugMode ? new CodeTypeReference(typeof(void)) : new CodeTypeReference(typeof(IEnumerator)),
                Name = output.VariableName
            };


            _.PushStatements(branchMethod.Statements);
            var actionNode = output.Node as ActionNode;
            if (actionNode != null)
            {
                actionNode.WriteActionOutputs(_);
            }

            if (output.ActionFieldInfo != null && output.ActionFieldInfo.IsBranch)
            {
                foreach (var item in output.ActionFieldInfo.DelegateMembers)
                {
                    branchMethod.Parameters.Add(new CodeParameterDeclarationExpression(item.MemberType.FullName, item.MemberName));

                    if (actionNode != null)
                    _._("{0}_{1} = {1}", actionNode.VariableName, item.MemberName);
            

                }
            }
    


            base.VisitBranch(output);
            if (DebugSystem.IsDebugMode)
                _._("yield break");

            var branchesChildItem = output.OutputTo<BranchesChildItem>();
            if (branchesChildItem != null)
            {
                _._("{0}()", branchesChildItem.Name);
            }
            _.PopStatements();
            _.CurrentDeclaration.Members.Add(branchMethod);

        }

        public override void VisitSequenceContainer(ISequenceNode handlerNode)
        {
            base.VisitSequenceContainer(handlerNode);
            _._comment("HANDLER: " + handlerNode.Name);
        }

        public override void VisitInput(IActionIn input)
        {
            base.VisitInput(input);
            //if (input.ActionFieldInfo == null) return;
            if (input.ActionFieldInfo != null)
            {
                if (input.ActionFieldInfo.IsGenericArgument) return;
                _.TryAddNamespace(input.ActionFieldInfo.MemberType.Namespace);
                var varDecl = new CodeMemberField(
                    input.VariableType.FullName.ToCodeReference(),
                    input.VariableName
                    )
                {
                    InitExpression = new CodeSnippetExpression(string.Format("default( {0} )", input.VariableType.FullName))
                };

                _.CurrentDeclaration.Members.Add(varDecl);

                var variableReference = input.Item;
                if (variableReference != null)
                    _.CurrentStatements.Add(new CodeAssignStatement(new CodeSnippetExpression(input.VariableName),
                        new CodeSnippetExpression(variableReference.VariableName)));
            }
            var inputVariable = input.InputFrom<VariableNode>();
            if (inputVariable != null)
            {
                var field = inputVariable.GetFieldStatement();
                if (_.CurrentDeclaration.Members.OfType<CodeMemberField>().All(p => p.Name != field.Name))
                {
                    _.CurrentDeclaration.Members.Add(inputVariable.GetFieldStatement());
                }

            }


        }
    }
}