using System;
using System.CodeDom;
using System.Linq;
using uFrame.Attributes;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.ECS.Actions;
using uFrame.ECS.Editor;
using uFrame.ECS.Components;
using uFrame.Editor;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;
using uFrame.IOC;

namespace uFrame.ECS.Templates
{
    public class EcsDyanmicActionTemplates : DiagramPlugin
    {
        public override bool Required
        {
            get { return true; }
        }
        public override void Initialize(UFrameContainer container)
        {

            //RegisteredTemplateGeneratorsFactory.RegisterTemplate<ComponentNode, AddComponentTemplate>();

        }
    }

    [TemplateClass(TemplateLocation.DesignerFile)]

    [RequiresNamespace("UnityEngine")]
    [RequiresNamespace("uFrame.Actions")]
    public class ActionTemplate<TNodeType> : IClassTemplate<TNodeType>, ITemplateCustomFilename where TNodeType : IDiagramNodeItem
    {
        public string Filename
        {
            get { return Path2.Combine("DesignerActions", ClassName + ".cs"); }
        }
        public string OutputPath
        {
            get { return Path2.Combine("DesignerActions", ClassName + ".cs"); }
        }

        public virtual bool CanGenerate
        {
            get { return true; }
        }

        public virtual void TemplateSetup()
        {
            Ctx.SetBaseType(typeof(UFAction));
            Ctx.CurrentDeclaration.CustomAttributes.Add(
              new CodeAttributeDeclaration(typeof(ActionTitle).ToCodeReference(),
                  new CodeAttributeArgument(
                      new CodePrimitiveExpression(ActionTitle))));
            Ctx.CurrentDeclaration.Name = ClassName;
        }

        protected virtual string ClassName
        {
            get { return "Add" + Ctx.Data.Name + "Action"; }
        }

        protected virtual string ActionTitle
        {
            get { return string.Format("{0}/{1}", Ctx.Data.Graph.Name, Ctx.Data.Name); }
        }

        public TemplateContext<TNodeType> Ctx { get; set; }
        public CodeMemberField AddIn(object type, string name)
        {
            var result = Ctx.CurrentDeclaration._public_(type, name);
            result.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(In))));
            return result;
        }
        public CodeMemberField AddOut(object type, string name)
        {
            var result = Ctx.CurrentDeclaration._public_(type, name);
            result.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(Out))));
            return result;
        }

        public CodeMemberField AddBranch(string name)
        {
            var result = Ctx.CurrentDeclaration._public_(typeof(Action), name);
            result.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(Out))));
            return result;
        }

    }


    public class AddComponentTemplate : ActionTemplate<ComponentNode>
    {
        protected override string ClassName
        {
            get { return string.Format("Add{0}Action", Ctx.Data.Name); }
        }

        protected override string ActionTitle
        {
            get { return string.Format("Add {0} Component", Ctx.Data.Name); }
        }

        [GenerateMethod(CallBase = true), AsOverride]
        public void Execute()
        {
            AddIn(typeof(EcsComponent), "Beside");

            foreach (var item in Ctx.Data.Properties)
            {
                AddIn(item.RelatedTypeName, item.Name);
            }

            Ctx._("Beside.gameObject.AddComponent<{0}>()", Ctx.Data.Name);
        }
    }

    public class PublishActionTemplate : ActionTemplate<EventNode>
    {
        protected override string ClassName
        {
            get { return string.Format("Publish{0}Action", Ctx.Data.Name); }
        }

        protected override string ActionTitle
        {
            get { return string.Format("Publish {0}", Ctx.Data.Name); }
        }

        public override bool CanGenerate
        {
            get { return !Ctx.Data.Dispatcher && !Ctx.Data.SystemEvent; }
        }

        [GenerateMethod(CallBase = true), AsOverride]
        public void Execute()
        {

            Ctx._("var evt = new {0}()", Ctx.Data.Name);

            foreach (var item in Ctx.Data.PersistedItems.OfType<ITypedItem>())
            {
                AddIn(item.RelatedTypeName, item.Name);
                Ctx._("evt.{0} = {0}", item.Name);
            }

            Ctx._("System.Publish(evt)");





        }
    }
}

