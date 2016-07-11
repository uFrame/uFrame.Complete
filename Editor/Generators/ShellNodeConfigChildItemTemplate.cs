using System.CodeDom;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Architect.Editor.Generators
{
    using Data;

    [TemplateClass(TemplateLocation.Both, ClassNameFormat = "{0}ChildItem")]
    public class ShellNodeConfigChildItemTemplate : GenericNodeChildItem,
        IClassTemplate<ShellNodeConfigSection>
    {
        public string OutputPath
        {
            get { return Path2.Combine("Editor", "ChildItems"); }
        }

        public bool CanGenerate
        {
            get { return Ctx.Data.SectionType == ShellNodeConfigSectionType.ChildItems; }
        }

        public void TemplateSetup()
        {
            //Ctx.TryAddNamespace("Invert.Core.GraphDesigner");
            Ctx.TryAddNamespace("uFrame.Editor.Configurations");
            Ctx.TryAddNamespace("uFrame.Editor.Core");
            Ctx.TryAddNamespace("uFrame.Editor.Graphs.Data");
            var i = new CodeTypeDeclaration(Ctx.Data.ReferenceClassName)
            {
                IsInterface = true,
                Attributes = MemberAttributes.Public,
                IsPartial = true,
            };
            i.BaseTypes.Add(new CodeTypeReference(typeof(IDiagramNodeItem)));
            i.BaseTypes.Add(new CodeTypeReference(typeof(IConnectable)));
            Ctx.Namespace.Types.Add(i);
            if (Ctx.IsDesignerFile)
            {
                if (Ctx.Data.IsTyped)
                    Ctx.SetBaseType(typeof(GenericTypedChildItem));

                foreach (var item in Ctx.Data.IncludedInSections)
                {
                    Ctx.AddInterface(item.ReferenceClassName);
                }
            }

        }

        public TemplateContext<ShellNodeConfigSection> Ctx { get; set; }

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