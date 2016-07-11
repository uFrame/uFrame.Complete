using System.CodeDom;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Architect.Editor.Generators
{
    using Data;

    [TemplateClass(TemplateLocation.Both, ClassNameFormat = "{0}")]
    public class ShellNodeConfigTemplateTemplate : IClassTemplate<ShellTemplateConfigNode>
    {

        public string OutputPath
        {
            get { return Path2.Combine("Editor", "Templates"); }
        }

        [GenerateProperty(TemplateLocation.DesignerFile)]
        public virtual string OutputPathProperty
        {
            get
            {
                Ctx.CurrentProperty.Name = "OutputPath";
                Ctx._("return \"{0}\"", Ctx.Data.OutputPath);
                return null;
            }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        [GenerateProperty(TemplateLocation.DesignerFile)]
        public virtual bool CanGenerateProperty
        {
            get
            {
                Ctx.CurrentProperty.Name = "CanGenerate";
                Ctx._("return true");
                return true;
            }
        }

        public void TemplateSetup()
        {
            //Ctx.TryAddNamespace("Invert.Core.GraphDesigner");
            Ctx.TryAddNamespace("System.CodeDom");
            Ctx.TryAddNamespace("uFrame.Editor.Compiling.CodeGen");
            Ctx.TryAddNamespace("uFrame.Editor.Configurations");
            Ctx.TryAddNamespace("uFrame.Editor.Graphs.Data");
            //Ctx.CurrentDecleration.IsPartial = true;
            //Ctx.CurrentDecleration.Name = Ctx.Data.Name;
            if (Ctx.IsDesignerFile)
            {
                Ctx.CurrentDeclaration.BaseTypes.Clear();
                Ctx.CurrentDeclaration.BaseTypes.Add(string.Format("IClassTemplate<{0}>", Ctx.Data.NodeConfig.ClassName));

                Ctx.CurrentDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(
                    new CodeTypeReference(typeof(TemplateClass)),
                    //new CodeAttributeArgument("OutputPath", new CodePrimitiveExpression(Ctx.Data.OutputPath)),
                    new CodeAttributeArgument("Location", new CodeSnippetExpression(string.Format("TemplateLocation.{0}", Ctx.Data.Files))),
                    new CodeAttributeArgument("AutoInherit", new CodePrimitiveExpression(Ctx.Data.AutoInherit)),
                    new CodeAttributeArgument("ClassNameFormat", new CodePrimitiveExpression(Ctx.Data.ClassNameFormat))
                    ));
            }
        }

        [GenerateMethod(TemplateLocation.Both)]
        public virtual void TemplateSetupMethod()
        {
            Ctx.CurrentMethod.Name = "TemplateSetup";
            if (Ctx.IsDesignerFile)
            {
                Ctx.PushStatements(Ctx._if("Ctx.IsDesignerFile").TrueStatements);
                Ctx._("Ctx.CurrentDecleration.BaseTypes.Clear()");
                if (!string.IsNullOrEmpty(Ctx.Data.TemplateBaseClass))
                {
                    Ctx._("Ctx.CurrentDecleration.BaseTypes.Add(new CodeTypeReference(\"{0}\"))", Ctx.Data.TemplateBaseClass);
                }
                Ctx.PopStatements();
            }

        }

        public TemplateContext<ShellTemplateConfigNode> Ctx { get; set; }

        [GenerateProperty("{0}"), WithField]
        public TemplateContext<GenericNode> CtxProperty
        {
            get
            {
                Ctx.CurrentProperty.Name = "Ctx";
                Ctx.SetTypeArgument(Ctx.Data.NodeConfig.ClassName);
                return null;
            }
        }
    }
}