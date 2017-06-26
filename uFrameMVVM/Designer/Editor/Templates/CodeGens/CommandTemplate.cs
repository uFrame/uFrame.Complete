using System;
using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;
using uFrame.MVVM.ViewModels;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both, ClassNameFormat = "{0}Command"), AsPartial]
    [AutoNamespaces]
    [NamespacesFromItems]
    public partial class CommandTemplate : IClassTemplate<CommandNode>, ITemplateCustomFilename
    {
        public TemplateContext<CommandNode> Ctx { get; set; }

        public string Filename
        {
            get
            {
                if (Ctx.Data.Name == null)
                {
                    throw new Exception(Ctx.Data.Name + " Graph name is empty");
                }
                return Ctx.IsDesignerFile ? Path2.Combine(Ctx.Data.Node.Graph.Name, "ViewModelCommands.designer.cs")
                                          : Path2.Combine(Ctx.Data.Node.Graph.Name + "/ViewModelCommands", Ctx.Data.Name + "Command.cs");
            }
        }

        // Replace by ITemplateCustomFilename's Filename
        public string OutputPath { get { return ""; } }

        public bool CanGenerate { get { return true; } }

        private CommandNode CommandNode
        {
            get
            {
                return (CommandNode) Ctx.NodeItem;
            }
        }

        public void TemplateSetup()
        {
            // Support inheritance
            Ctx.CurrentDeclaration.BaseTypes.Clear();
            if (!CommandNode.IsStruct)
            {
                if (CommandNode.BaseNode != null)
                {
                    Ctx.CurrentDeclaration.BaseTypes.Add((CommandNode.BaseNode.Name + "Command").ToCodeReference());
                }
                else
                {
                    Ctx.SetBaseType(typeof(ViewModelCommand));
                }
            }
            else
            {
                if (CommandNode.BaseNode != null)
                    throw new TemplateException(Ctx.Item.Name + " is Struct, but BaseNode = " + CommandNode.BaseNode.FullName);

                Ctx.CurrentDeclaration.IsClass = false;
                Ctx.CurrentDeclaration.IsStruct = true;

                if (Ctx.IsDesignerFile)
                {
                    Ctx.CurrentDeclaration.BaseTypes.Add(typeof(IViewModelCommand));
                }
            }

            foreach (var property in Ctx.Data.ChildItemsWithInherited.OfType<ITypedItem>())
            {
                var type = InvertApplication.FindTypeByNameExternal(property.RelatedTypeName);
                if (type == null)
                    continue;

                Ctx.TryAddNamespace(type.Namespace);
            }

            if (!Ctx.IsDesignerFile)
            {
                Ctx.CurrentDeclaration.BaseTypes.Clear();
            }
            else
            {
                Ctx.CurrentDeclaration.Name = string.Format("{0}Command", Ctx.Data.Name);
            }
        }
    }

    [RequiresNamespace("UnityEngine")]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.MVVM")]
    [RequiresNamespace("uFrame.MVVM.Bindings")]
    [RequiresNamespace("uFrame.Kernel.Serialization")]
    [RequiresNamespace("uFrame.MVVM.ViewModels")]
    public partial class CommandTemplate
    {
        public bool IsStruct
        {
            get
            {
                return ((CommandNode) Ctx.NodeItem).IsStruct;
            }
        }

        [ForEach("Properties"), GenerateProperty, WithField, WithNameFormat("{0}")]
        public _ITEMTYPE_ _Name_Property { get; set; }

        [ForEach("Collections"), GenerateProperty, WithField, WithNameFormat("{0}")]
        public List<_ITEMTYPE_> _Name_Collection { get; set; }

        [If("IsStruct"), GenerateProperty, WithField]
        public ViewModel Sender { get; set; }
    }
}


