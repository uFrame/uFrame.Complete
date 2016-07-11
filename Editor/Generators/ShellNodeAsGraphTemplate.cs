using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Architect.Editor.Generators
{
    using Data;

    [TemplateClass(TemplateLocation.Both, ClassNameFormat = "{0}Graph")]
    public class ShellNodeAsGraphTemplate : GenericGraphData<GenericNode>, IClassTemplate<ShellNodeConfig>
    {
        public string OutputPath
        {
            get { return Path2.Combine("Editor", "Graphs"); }
        }

        public bool CanGenerate
        {
            get { return Ctx.Data.IsGraphType; }
        }

        public void TemplateSetup()
        {
            //Ctx.TryAddNamespace("Invert.Core.GraphDesigner");
            Ctx.TryAddNamespace("uFrame.Editor.Configurations");
            Ctx.TryAddNamespace("uFrame.Editor.Core");
            Ctx.TryAddNamespace("uFrame.Editor.Graphs.Data");
            if (Ctx.IsDesignerFile)
            {
                Ctx.SetBaseType("GenericGraphData<{0}>", Ctx.Data.ClassName);
            }
        }

        public TemplateContext<ShellNodeConfig> Ctx { get; set; }
    }
}