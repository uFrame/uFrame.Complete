using uFrame.Editor.Compiling.CodeGen;
using uFrame.Architect.Editor.Data;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.Architect.Editor.Generators
{
    [TemplateClass(TemplateLocation.Both, ClassNameFormat = "{0}NodeViewModel", AutoInherit = false)]
    public class ShellNodeTypeViewModelTemplate : GenericNodeViewModel<GenericNode>, IClassTemplate<ShellNodeTypeNode>
    {
        public string OutputPath
        {
            get { return Path2.Combine("Editor", "ViewModels"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            if (Ctx.IsDesignerFile)
            {
                if (Ctx.Data.BaseNode != null)
                {
                    Ctx.SetBaseType(Ctx.Data.BaseNode.Name + "NodeViewModel");
                }
                else
                {
                    Ctx.SetBaseTypeArgument(Ctx.Data.ClassName);
                }

            }

        }

        public TemplateContext<ShellNodeTypeNode> Ctx { get; set; }

        // For templating
        public ShellNodeTypeViewModelTemplate()
            : base()
        {
        }

        public ShellNodeTypeViewModelTemplate(GenericNode graphItemObject, DiagramViewModel diagramViewModel)
            : base(graphItemObject, diagramViewModel)
        {
        }

        [GenerateConstructor(TemplateLocation.Both, "graphItemObject", "diagramViewModel")]
        public void ViewModelConstructor(GenericNode graphItemObject, DiagramViewModel diagramViewModel)
        {
            Ctx.CurrentConstructor.Parameters[0].Type = Ctx.Data.ClassName.ToCodeReference();

        }

    }
}