using uFrame.Architect.Editor.Data;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Architect.Editor.Generators
{
    [TemplateClass(TemplateLocation.Both, ClassNameFormat = "{0}ChildItem")]
    public class ShellChildItemTemplate : GenericNodeChildItem,
        IClassTemplate<ShellNodeChildTypeNode>
    {
        public string OutputPath
        {
            get { return Path2.Combine("Editor", "ChildItems"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            //Ctx.TryAddNamespace("Invert.Core.GraphDesigner");
            Ctx.TryAddNamespace("uFrame.Editor.Graphs.Data");
            if (Ctx.IsDesignerFile)
            {
                if (Ctx.Data["Typed"])
                    Ctx.SetBaseType(typeof(GenericTypedChildItem));

                foreach (var item in Ctx.Data.IncludedInSections)
                {
                    Ctx.AddInterface(item.ReferenceClassName);
                }
            }

        }

        public TemplateContext<ShellNodeChildTypeNode> Ctx { get; set; }

        [GenerateProperty(TemplateLocation.DesignerFile)]
        public override bool AllowMultipleInputs
        {
            get
            {
                Ctx._("return {0}", Ctx.Data.AllowMultipleInputs ? "true" : "false");
                return base.AllowMultipleInputs;
            }
        }
        [GenerateProperty(TemplateLocation.DesignerFile)]
        public override bool AllowMultipleOutputs
        {
            get
            {
                Ctx._("return {0}", Ctx.Data.AllowMultipleOutputs ? "true" : "false");
                return base.AllowMultipleOutputs;
            }
        }
    }
}
