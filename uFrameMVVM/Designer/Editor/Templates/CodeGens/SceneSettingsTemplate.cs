using System;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both, "{0}Settings")]
    [AutoNamespaces]
    [NamespacesFromItems]
    public partial class SceneSettingsTemplate : IClassTemplate<SceneTypeNode>, ITemplateCustomFilename
    {
        public TemplateContext<SceneTypeNode> Ctx { get; set; }

        public string Filename
        {
            get
            {
                if (Ctx.Data.Name == null)
                {
                    throw new Exception(Ctx.Data.Name + " Graph name is empty");
                }
                return Ctx.IsDesignerFile ? Path2.Combine(Ctx.Data.Graph.Name, "SceneSettings.designer.cs")
                                          : Path2.Combine(Ctx.Data.Graph.Name + "/ScenesSettings", Ctx.Data.Name + "Setting.cs");
            }
        }

        // Replace by ITemplateCustomFilename's Filename
        public string OutputPath { get { return ""; } }

        public bool CanGenerate { get { return true; } }

        public void TemplateSetup()
        {
            if (!Ctx.IsDesignerFile) return;
            Ctx.CurrentDeclaration.BaseTypes.Clear();
            Ctx.SetBaseType("SceneSettings<{0}>", Ctx.Data.Name);
        }
    }

    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.Kernel.Serialization")]
    public partial class SceneSettingsTemplate
    {

    }
}


