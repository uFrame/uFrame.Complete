using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Architect.Editor.Data;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Architect.Editor.Generators
{
    [TemplateClass(TemplateLocation.Both, ClassNameFormat = "{0}")]
    //[RequiresNamespace("Invert.Data")]
    public class ShellSlotItemTemplate : GenericSlot, IClassTemplate<IShellSlotType>
    {
        [GenerateProperty]
        public override string Name
        {
            get
            {
                Ctx._("return \"{0}\"", Ctx.Data.Name);
                return null;
            }
            set { }
        }

        [GenerateProperty(TemplateLocation.DesignerFile)]
        public override bool AllowMultipleInputs
        {
            get
            {
                Ctx._("return {0}", Ctx.Data.AllowMultiple && Ctx.Data is ShellNodeConfigInput ? "true" : "false");
                return base.AllowMultipleInputs;
            }
        }
        [GenerateProperty(TemplateLocation.DesignerFile)]
        public override bool AllowMultipleOutputs
        {
            get
            {
                Ctx._("return {0}", Ctx.Data.AllowMultiple && Ctx.Data is ShellNodeConfigOutput ? "true" : "false");
                return base.AllowMultipleOutputs;
            }
        }
        [GenerateProperty(TemplateLocation.DesignerFile)]
        public override bool AllowSelection
        {
            get
            {
                Ctx._("return {0}", Ctx.Data.AllowSelection ? "true" : "false");
                return false;
            }
        }
        public string OutputPath
        {
            get { return Path.Combine("Editor", "Slots"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }
        [GenerateMethod(CallBase = false)]
        public override IEnumerable<IValueItem> GetAllowed()
        {
            Ctx._(string.Format("return Repository.AllOf<{0}>().OfType<IValueItem>();", Ctx.Data.ReferenceClassName));
            yield break;
        }

        public void TemplateSetup()
        {
            //Ctx.TryAddNamespace("Invert.Core.GraphDesigner");
            Ctx.TryAddNamespace("uFrame.Editor.Database.Data");
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
                if (Ctx.Data.IsOutput)
                {
                    if (Ctx.Data.AllowMultiple)
                    {
                        Ctx.SetBaseType("MultiOutputSlot<{0}>", Ctx.Data.ReferenceClassName);
                    }
                    else
                    {
                        Ctx.SetBaseType("SingleOutputSlot<{0}>", Ctx.Data.ReferenceClassName);
                    }
                }
                else
                {
                    if (Ctx.Data.AllowMultiple)
                    {
                        Ctx.SetBaseType("MultiInputSlot<{0}>", Ctx.Data.ReferenceClassName);
                    }
                    else
                    {
                        Ctx.SetBaseType("SingleInputSlot<{0}>", Ctx.Data.ReferenceClassName);
                    }
                }

                foreach (var item in Ctx.Data.IncludedInSections)
                {
                    Ctx.AddInterface(item.ReferenceClassName);
                }
            }


        }

        public TemplateContext<IShellSlotType> Ctx { get; set; }
    }
}