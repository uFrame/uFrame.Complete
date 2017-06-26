using System;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;
using uFrame.Editor.Core;
using uFrame.MVVM.ViewModels;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both, ClassNameFormat = "{0}Command"), AsPartial]
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
                //return Path2.Combine(Ctx.Data.Node.Graph.Name, "ViewModelCommands.designer.cs");

                return Ctx.IsDesignerFile ? Path2.Combine(Ctx.Data.Node.Graph.Name, "ViewModelCommands.designer.cs")
                                          : Path2.Combine(Ctx.Data.Node.Graph.Name + "/ViewModelCommands", Ctx.Data.Name + "Command.cs");
            }
        }

        // Replace by ITemplateCustomFilename's Filename
        public string OutputPath { get { return ""; } }

        private CommandsChildItem CommandsChildItem
        {
            get
            {
                return (CommandsChildItem) Ctx.NodeItem;
            }
        }

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

            if (!Ctx.IsDesignerFile) Ctx.CurrentDeclaration.BaseTypes.Clear();

            // Support inheritance
            Ctx.CurrentDeclaration.BaseTypes.Clear();
            if (!CommandsChildItem.IsStruct)
            {
                Ctx.SetBaseType(typeof(ViewModelCommand));
            }
            else
            {
                Ctx.CurrentDeclaration.IsClass = false;
                Ctx.CurrentDeclaration.IsStruct = true;

                if (Ctx.IsDesignerFile)
                {
                    Ctx.CurrentDeclaration.BaseTypes.Add(typeof(IViewModelCommand));
                }
            }
        }
    }

    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.MVVM")]
    [RequiresNamespace("uFrame.Kernel.Serialization")]
    public partial class ViewModelCommandClassTemplate
    {
        public bool IsStruct
        {
            get
            {
                return CommandsChildItem.IsStruct;
            }
        }

        public bool HasArgument
        {
            get
            {
                return Ctx.Data.HasArgument;
            }
        }

        [GenerateProperty, WithField, If("HasArgument")]
        public _ITEMTYPE_ Argument { get; set; }

        [If("IsStruct"), GenerateProperty, WithField]
        public new ViewModel Sender { get; set; }
    }
}