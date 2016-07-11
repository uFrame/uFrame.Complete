using System;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Architect.Editor.Data;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI.ViewModels;
using uFrame.Editor.Platform;

namespace uFrame.Architect.Editor.Generators
{
    [TemplateClass(TemplateLocation.Both, ClassNameFormat = "{0}NodeViewModel", AutoInherit = false)]
    public class ShellNodeConfigViewModelTemplate : GenericNodeViewModel<GenericNode>, IClassTemplate<ShellNodeConfig>
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
            Ctx.TryAddNamespace("uFrame.Editor.Graphs.Data");
            Ctx.TryAddNamespace("uFrame.Editor.GraphUI.ViewModels");
            Ctx.TryAddNamespace("uFrame.Editor.Platform");

            if (Ctx.IsDesignerFile)
            {
                if (Ctx.Data.BaseNode != null)
                {
                    Ctx.SetBaseType(Ctx.Data.BaseNode.Name + "NodeViewModel");
                }
                else
                {
                    // Ctx.SetType(Ctx.Data.ClassName + "ViewModel");
                }


            }

        }
        [GenerateProperty]
        public override NodeStyle NodeStyle
        {
            get
            {
                Ctx._("return NodeStyle.{0}", Enum.GetName(typeof(NodeStyle), Ctx.Data.NodeStyle));
                return NodeStyle.Normal;
            }
        }

        public TemplateContext<ShellNodeConfig> Ctx { get; set; }

        // For templating
        public ShellNodeConfigViewModelTemplate()
            : base()
        {
        }

        public ShellNodeConfigViewModelTemplate(GenericNode graphItemObject, DiagramViewModel diagramViewModel)
            : base(graphItemObject, diagramViewModel)
        {
        }

        [GenerateConstructor(TemplateLocation.Both, "graphItemObject", "diagramViewModel"), Inside(TemplateLocation.Both)]
        public void ViewModelConstructor(GenericNode graphItemObject, DiagramViewModel diagramViewModel)
        {
            Ctx.CurrentConstructor.Parameters[0].Type = Ctx.Data.ClassName.ToCodeReference();

        }

    }
}