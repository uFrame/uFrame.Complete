using System;
using System.CodeDom;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both, "{0}Loader")]
    [AutoNamespaces]
    [NamespacesFromItems]
    public partial class SceneLoaderTemplate : IClassTemplate<SceneTypeNode>, ITemplateCustomFilename 
    {
        public TemplateContext<SceneTypeNode> Ctx { get; set;}

        public string Filename
        {
            get
            {
                if(Ctx.Data.Name == null)
                {
                    throw new Exception(Ctx.Data.Name + " Graph name is empty");
                }
                return Ctx.IsDesignerFile ? Path2.Combine(Ctx.Data.Graph.Name, "SceneLoader.designer.cs") 
                                          : Path2.Combine(Ctx.Data.Graph.Name + "/SceneLoaders", Ctx.Data.Name + "Loader.cs");
            }
        }

        // Replace by ITemplateCustomFilename's Filename
        public string OutputPath { get { return ""; } }

        public bool CanGenerate { get { return true; } }

        public void TemplateSetup()
        {
            if (!Ctx.IsDesignerFile) return;
            Ctx.CurrentDeclaration.BaseTypes.Clear();
            Ctx.CurrentDeclaration.BaseTypes.Add(string.Format("SceneLoader<{0}>", Ctx.Data.Name));
        }
    }

    [RequiresNamespace("uFrame.IOC")]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.MVVM")]
    [RequiresNamespace("uFrame.Kernel.Serialization")]
    [RequiresNamespace("UnityEngine")]
    public partial class SceneLoaderTemplate
    {
        [GenerateMethod(CallBase = false), Inside(TemplateLocation.Both)]
        protected virtual void LoadScene(object scene, object progressDelegate)
        {
            Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.Data.Name);
            Ctx.CurrentMethod.Parameters[1].Type = new CodeTypeReference("Action<float, string>");
            Ctx.CurrentMethod.Attributes |= MemberAttributes.Override;
            Ctx.CurrentMethod.ReturnType = "IEnumerator".ToCodeReference();
            Ctx._("yield break");
        }

        [GenerateMethod(CallBase = false), Inside(TemplateLocation.Both)]
        protected virtual void UnloadScene(object scene, object progressDelegate)
        {
            Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.Data.Name);
            Ctx.CurrentMethod.Parameters[1].Type = new CodeTypeReference("Action<float, string>");
            Ctx.CurrentMethod.Attributes |= MemberAttributes.Override;
            Ctx.CurrentMethod.ReturnType = "IEnumerator".ToCodeReference();
            Ctx._("yield break");
        }
    }
}

