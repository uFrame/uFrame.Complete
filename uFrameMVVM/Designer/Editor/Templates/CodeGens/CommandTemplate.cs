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
    [TemplateClass(TemplateLocation.DesignerFile, ClassNameFormat = "{0}Command"), AsPartial]
    [AutoNamespaces]
    [NamespacesFromItems]
    public partial class CommandTemplate :IClassTemplate<CommandNode>, ITemplateCustomFilename 
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
                return Path2.Combine(Ctx.Data.Node.Graph.Name, "ViewModelCommands.designer.cs");
            }
        }

        // Replace by ITemplateCustomFilename's Filename
        public string OutputPath { get { return ""; } }

        public bool CanGenerate { get { return true; } }

        public void TemplateSetup()
        {
            foreach (var property in Ctx.Data.ChildItemsWithInherited.OfType<ITypedItem>())
            {
                var type = InvertApplication.FindTypeByNameExternal(property.RelatedTypeName);
                if (type == null)
                    continue;

                Ctx.TryAddNamespace(type.Namespace);
            }
        }
    }

    [ForceBaseType(typeof(ViewModelCommand))]
    [RequiresNamespace("UnityEngine")]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.MVVM")]
    [RequiresNamespace("uFrame.MVVM.Bindings")]
    [RequiresNamespace("uFrame.Kernel.Serialization")]
    [RequiresNamespace("uFrame.MVVM.ViewModels")]
    public partial class CommandTemplate
    {
        [ForEach("Properties"), GenerateProperty, WithField]
        public _ITEMTYPE_ _PropertyName_ { get; set; }

        [ForEach("Collections"), GenerateProperty, WithField]
        public List<_ITEMTYPE_> _CollectionName_ { get; set; }
    }
}


