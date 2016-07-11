using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;
using uFrame.MVVM.ViewModels;
using uFrame.MVVM.Views;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both)]
    public partial class ViewComponentTemplate : IClassTemplate<ViewComponentNode>, ITemplateCustomFilename
    {
        public TemplateContext<ViewComponentNode> Ctx { get; set; }

        public string Filename
        {
            get
            {
                if (Ctx.Data.Name == null)
                {
                    throw new Exception(Ctx.Data.Name + " Graph name is empty");
                }
                return Ctx.IsDesignerFile ? Path2.Combine(Ctx.Data.Graph.Name, "ViewComponents.designer.cs")
                                          : Path2.Combine(Ctx.Data.Graph.Name + "/ViewComponents", Ctx.Data.Name + ".cs");
            }
        }

        // Replace by ITemplateCustomFilename's Filename
        public string OutputPath { get { return ""; } }

        public bool CanGenerate { get { return true; } }

        public virtual void TemplateSetup()
        {
            if (Ctx.IsDesignerFile)
            {
                Ctx.SetBaseType(typeof(ViewComponent));
            }

            foreach (var property in Ctx.Data.View.Element.PersistedItems.OfType<ITypedItem>())
            {
                var type = InvertApplication.FindTypeByNameExternal(property.RelatedTypeName);
                if (type == null) continue;

                Ctx.TryAddNamespace(type.Namespace);
            }

            Ctx.AddIterator("ExecuteCommandOverload", _ => _.View.Element.LocalCommands);
        }

    }

    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.MVVM")]
    [RequiresNamespace("uFrame.MVVM.Services")]
    [RequiresNamespace("uFrame.MVVM.ViewModels")]
    [RequiresNamespace("uFrame.MVVM.Bindings")]
    [RequiresNamespace("uFrame.Kernel.Serialization")]
    [RequiresNamespace("UniRx")]
    [RequiresNamespace("UnityEngine")]
    public partial class ViewComponentTemplate
    {
        public string ElementName
        {
            get
            {
                return Ctx.Data.View.Element.Name;
            }
        }

        [GenerateProperty]
        public virtual object _ElementName_
        {
            get
            {
                Ctx.CurrentProperty.Type = Ctx.Data.View.Element.Name.AsViewModel().ToCodeReference();
                Ctx._("return ({0})this.View.ViewModelObject", Ctx.Data.View.Element.Name.AsViewModel());
                return null;
            }
        }

        public IEnumerable<CommandsChildItem> CommandsWithArguments
        {
            get
            {
                return Ctx.Data.View.Element.InheritedCommandsWithLocal
                          .Where(p => ((CommandsChildItem)p).HasArgument && p.OutputCommand == null);
            }
        }

        public IEnumerable<CommandsChildItem> Commands
        {
            get
            {
                return Ctx.Data.View.Element.InheritedCommandsWithLocal
                            .Where(p => !((CommandsChildItem)p).HasArgument);
            }
        }

        [ForEach("Commands"), GenerateMethod]
        public void Execute_Name_()
        {
            Ctx._("{0}.{1}.OnNext(new {1}Command() {{ Sender = {0} }})",
                Ctx.Data.View.Element.Name, Ctx.Item.Name);
        }


        [ForEach("CommandsWithArguments"), GenerateMethod(CallBase = false)]
        public void Execute_Name2_(object arg)
        {
            Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.TypedItem.RelatedTypeName);
            Ctx._("{0}.{1}.OnNext(new {1}Command() {{ Sender = {0}, Argument = arg }})", Ctx.Data.View.Element.Name, Ctx.Item.Name);
        }

        [GenerateMethod("Execute{0}", TemplateLocation.DesignerFile, false)]
        public void ExecuteCommandOverload(ViewModelCommand command)
        {
            Ctx.CurrentMethod.Parameters[0].Type = (Ctx.Item.Name + "Command").ToCodeReference();
            Ctx._("command.Sender = {0}", Ctx.Data.View.Element.Name);
            Ctx._("{0}.{1}.OnNext(command)", Ctx.Data.View.Element.Name, Ctx.Item.Name);
            //Ctx._("this.ExecuteCommand({0}.{1})", Ctx.Data.Element.Name, Ctx.Item.Name);
        }
    }
}