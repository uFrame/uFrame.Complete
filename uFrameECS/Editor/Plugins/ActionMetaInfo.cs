using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using uFrame.Attributes;
using uFrame.ECS.Editor;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Core;
using uFrame.Editor.DebugSystem;
using uFrame.Editor.Graphs.Data.Types;

namespace uFrame.ECS.Editor
{
    public interface IActionMetaInfo : ITypeInfo
    {
        IEnumerable<string> CategoryPath { get; }
        bool IsEditorClass { get; set; }
        uFrameCategory Category { get; set; }
        ActionDescription DescriptionAttribute {get;set;}
        bool IsAsync { get; }
        void WriteCode(TemplateContext ctx, ActionNode node);
    }

    public class ActionMetaInfo : SystemTypeInfo, IItem, IActionMetaInfo
    {
        private ActionDescription _description;
        private ActionTitle _title;
        private List<ActionFieldInfo> _actionFields;
        private uFrameCategory _category;


        public ActionTitle TitleAttribute
        {
            get { return _title ?? (_title = MetaAttributes.OfType<ActionTitle>().FirstOrDefault()); }
            set { _title = value; }
        }


        public override string Title
        {
            get
            {
                if (TitleAttribute == null)
                    return SystemType.Name;

                return TitleAttribute.Title;
            }
        }


        public ActionDescription DescriptionAttribute
        {
            get { return _description ?? (_description = MetaAttributes.OfType<ActionDescription>().FirstOrDefault()); }
            set { _description = value; }
        }
        public uFrameCategory Category
        {
            get { return _category ?? (_category = SystemType.GetCustomAttributes(typeof(uFrameCategory), true).OfType<uFrameCategory>().FirstOrDefault()); }
            set { _category = value; }
        }

        public override string Description
        {
            get { return DescriptionAttribute != null ? DescriptionAttribute.Description : null; }
        }

        public bool IsAsync
        {
            get
            {
                if (MetaAttributes == null) return false;
                return MetaAttributes.OfType<AsyncAction>().Any();
            }
        }

        public virtual void WriteCode(TemplateContext ctx, ActionNode node)
        {
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

        public IEnumerable<string> CategoryPath
        {
            get
            {
                if (Category == null)
                {
                    yield break;
                }
                foreach (var item in Category.Title)
                {
                    yield return item;
                }
            }
        }
        public List<ActionFieldInfo> ActionFields
        {
            get { return _actionFields ?? (_actionFields = new List<ActionFieldInfo>()); }
            set { _actionFields = value; }
        }

        public ActionMetaAttribute[] MetaAttributes { get; set; }
        public bool IsEditorClass { get; set; }

        public ActionMetaInfo(Type systemType) : base(systemType)
        {
        }


        public override IEnumerable<IMemberInfo> GetMembers()
        {
            return ActionFields.OfType<IMemberInfo>();
        }
    }

    public class ActionMethodMetaInfo : ActionMetaInfo
    {
        public override string Identifier
        {
            get
            {
                return string.Format("{0}.{1}", FullName, ShortName);
            }
        }

        public string ShortName
        {
            get
            {
                return string.Format("{0}({1})", Method.Name, string.Join(",", Method.GetParameters().Select(p => p.Name).ToArray()));
            }
        }
        private string _title1;
        public MethodInfo Method { get; set; }

        public bool IsConverter
        {
            get { return this.TitleAttribute is ActionTypeConverter; }
        }

        public override string Title
        {
            get
            {
                return ShortName;// TitleAttribute == null ? Method.Name : TitleAttribute.Title ;
            }
        }

        public IActionFieldInfo ConvertFrom
        {
            get { return ActionFields.First(p => !p.IsReturn); }
        }

        public IActionFieldInfo ConvertTo
        {
            get { return ActionFields.First(p => p.IsReturn); }
        }

        public override string FullName
        {
            get { return base.FullName + "." + Method.Name; }
        }

        public ActionFieldInfo InstanceInfo { get; set; }

        //public MethodInfo Method { get; set; }
        public ActionMethodMetaInfo(Type systemType) : base(systemType)
        {
        }

        public override void WriteCode(TemplateContext ctx, ActionNode node)
        {
            var codeMethodReferenceExpression = new CodeMethodReferenceExpression(
                new CodeSnippetExpression(SystemType.FullName),
                Method.Name);
            if (this.InstanceInfo != null)
            {
                var instanceVar = node.InputVars.FirstOrDefault(p => p.ActionFieldInfo == this.InstanceInfo);
                if (instanceVar != null)
                {
                    codeMethodReferenceExpression = new CodeMethodReferenceExpression(new CodeSnippetExpression(instanceVar.VariableName),Method.Name);
                }
                
            }
            var _currentActionInvoker =
                new CodeMethodInvokeExpression(
                    codeMethodReferenceExpression);
           
            foreach (var x in Method.GetParameters())
            {
                var item = node.GraphItems.OfType<IActionItem>().FirstOrDefault(p => p.Name == x.Name);
                var input = item as IActionIn;
                if (input != null)
                {
                    if (input.ActionFieldInfo == InstanceInfo) continue;

                    if (input.ActionFieldInfo.IsGenericArgument)
                    {
                    }
                    else
                    {
                        _currentActionInvoker.Parameters.Add(
                            new CodeSnippetExpression((input.ActionFieldInfo.IsByRef ? "ref " : string.Empty) +
                                                      string.Format("{0}", input.VariableName)));
                    }
                }
                var @out = item as ActionOut;
                if (@out != null)
                {
                    if (@out.ActionFieldInfo != null && @out.ActionFieldInfo.IsReturn)
                    {
                 
                        continue;
                    }
                    _currentActionInvoker.Parameters.Add(
                        new CodeSnippetExpression(string.Format("out {0}", @out.VariableName)));
                }
                var branch = item as ActionBranch;
                if (branch != null)
                {
                    if (DebugSystem.IsDebugMode)
                    {
                        _currentActionInvoker.Parameters.Add(
                            new CodeSnippetExpression(string.Format("()=> {{ System.StartCoroutine({0}()); }}",
                                branch.VariableName)));
                    }
                    else
                    {
                        _currentActionInvoker.Parameters.Add(
                            new CodeSnippetExpression(string.Format("{0}", branch.VariableName)));
                    }
                }
            }

            var resultOut = node.GraphItems.OfType<IActionItem>().FirstOrDefault(p => p.ActionFieldInfo != null && p.ActionFieldInfo.IsReturn);
            if (resultOut == null)
            {
                ctx.CurrentStatements.Add(_currentActionInvoker);
            }
            else
            {
                var assignResult = new CodeAssignStatement(
                    new CodeSnippetExpression(resultOut.VariableName), _currentActionInvoker);
                ctx.CurrentStatements.Add(assignResult);
            }
        }
    }



}
