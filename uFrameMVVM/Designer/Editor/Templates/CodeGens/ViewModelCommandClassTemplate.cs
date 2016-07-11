using System;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;
using uFrame.Editor.Core;
using uFrame.MVVM.ViewModels;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.DesignerFile, ClassNameFormat = "{0}Command"), AsPartial]
    public partial class ViewModelCommandClassTemplate : ViewModelCommand, IClassTemplate<CommandsChildItem>, ITemplateCustomFilename
    {
        public TemplateContext<CommandsChildItem> Ctx { get; set; }

        public string Filename
        {
            get
            {
                if (Ctx.Data.Name == null)
                {
                    throw new Exception(Ctx.Data.Name + " Graph name is empty");
                }
                //return Path2.Combine("Commands.designer", Ctx.Data.Name + "Command.designer.cs");
                return Path2.Combine(Ctx.Data.Node.Graph.Name, "ViewModelCommands.designer.cs");
            }
        }

        // Replace by ITemplateCustomFilename's Filename
        public string OutputPath { get { return ""; } }

        public bool CanGenerate
        {
            get
            {
                if (Ctx.Data.OutputCommand != null) return false;
                return true;
            }
        }

        public void TemplateSetup()
        {
            var type = InvertApplication.FindTypeByNameExternal(Ctx.Data.RelatedTypeName);
            if (type != null)
            {
                Ctx.TryAddNamespace(type.Namespace);
            }
            else
            {
                type = InvertApplication.FindType(Ctx.Data.RelatedTypeName);
                if (type != null)
                    Ctx.TryAddNamespace(type.Namespace);
            }
            Ctx.CurrentDeclaration.Name = Ctx.Data.Name + "Command";
            Ctx.AddCondition("Argument", _ => _.HasArgument);
        }
    }

    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.MVVM")]
    [RequiresNamespace("uFrame.Kernel.Serialization")]
    public partial class ViewModelCommandClassTemplate
    {
        public bool HasArgument
        {
            get
            {
                return Ctx.Data.HasArgument;
            }
        }

        [GenerateProperty, WithField, If("HasArgument")]
        public _ITEMTYPE_ Argument { get; set; }
    }
}