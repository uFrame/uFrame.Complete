using System;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.DesignerFile, "{0}Loader"), AsPartial]
    [RequiresNamespace("uFrame.Kernel")]
    [WithMetaInfo]
    [AutoNamespaces]
    [NamespacesFromItems]
    public partial class SystemLoaderPartialTemplate : IClassTemplate<SubSystemNode>, ITemplateCustomFilename 
    {
        public TemplateContext<SubSystemNode> Ctx { get; set; }

        public string Filename
        {
            get
            {
                if (Ctx.Data.Name == null)
                {
                    throw new Exception(Ctx.Data.Name + " Graph name is empty");
                }
                return Ctx.IsDesignerFile ? Path2.Combine("Systems.designer", Ctx.Data.Name + "Loader.designer.cs") 
                                          : Path2.Combine("Systems", Ctx.Data.Name + "Loader.cs");
            }
        }

        // Replace by ITemplateCustomFilename's Filename
        public string OutputPath { get { return ""; } }

        public bool CanGenerate { get { return true; } }

        public void TemplateSetup()
        {
            Ctx.CurrentDeclaration.Name = Ctx.Data.Name + "Loader";
            Ctx.CurrentDeclaration.BaseTypes.Clear();
            Ctx.CurrentDeclaration.BaseTypes.Add((Ctx.Data.Name + "LoaderBase").ToCodeReference());
        }
    }
}

