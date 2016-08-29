using System;
using System.CodeDom;
using UnityEngine;
using uFrame.Kernel;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both, "{0}")]
    [AutoNamespaces]
    [NamespacesFromItems]
    public partial class SceneTemplate : IClassTemplate<SceneTypeNode>, ITemplateCustomFilename
    {
        public TemplateContext<SceneTypeNode> Ctx{ get; set;}
      
        public string Filename
        {
            get
            {
                if (Ctx.Data.Name == null)
                {
                    throw new Exception(Ctx.Data.Name + " Graph name is empty");
                }
                return Ctx.IsDesignerFile ? Path2.Combine(Ctx.Data.Graph.Name, "Scenes.designer.cs") 
                                          : Path2.Combine(Ctx.Data.Graph.Name + "/Scenes", Ctx.Data.Name + ".cs");
            }
        }

        // Replace by ITemplateCustomFilename's Filename
        public string OutputPath { get { return ""; } }

        public bool CanGenerate { get { return true; } }

        public void TemplateSetup()
        {
            if(Ctx.IsDesignerFile)
            {
                Ctx.CurrentDeclaration.BaseTypes.Add(typeof(MonoBehaviour).ToCodeReference());
            }
        }


    }

    [ForceBaseType(typeof(Scene))]
    public partial class SceneTemplate
    {
        [GenerateProperty]
        public virtual string DefaultKernelScene
        {
            get
            {
                Ctx.CurrentProperty.Attributes = MemberAttributes.Override | MemberAttributes.Public;
                Ctx._("return \"{0}KernelScene\"", Ctx.Data.Graph.Namespace);
                return null;
            }
        }

        [GenerateProperty]
        public virtual object Settings
        {
            get
            {
                Ctx.SetType(string.Format("{0}Settings", Ctx.Data.Name).ToCodeReference());
                Ctx._(string.Format("return _SettingsObject as {0}Settings", Ctx.Data.Name));
                return null;
            }
            set
            { Ctx._(string.Format("_SettingsObject = value")); }
        }
    }
}


