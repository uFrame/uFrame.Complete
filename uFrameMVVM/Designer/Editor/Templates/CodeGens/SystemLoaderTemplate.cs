using System;
using System.CodeDom;
using System.Linq;
using uFrame.Kernel;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;
using uFrame.IOC;
using uFrame.MVVM.ViewModels;
using UnityEngine;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both, "{0}Loader")]
    [AutoNamespaces]
    [NamespacesFromItems]
    public partial class SystemLoaderTemplate : IClassTemplate<SubSystemNode>, ITemplateCustomFilename
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
                return Ctx.IsDesignerFile ? Path2.Combine(Ctx.Data.Graph.Name + "/SystemLoaders.designer", Ctx.Data.Name + "Loader.designer.cs")
                                          : Path2.Combine(Ctx.Data.Graph.Name + "/SystemsLoaders", Ctx.Data.Name + "Loader.cs");
            }
        }

        // Replace by ITemplateCustomFilename's Filename
        public string OutputPath { get { return ""; } }

        public bool CanGenerate { get { return true; } }

        public void TemplateSetup()
        {
            foreach (var property in Ctx.Data.PersistedItems.OfType<ITypedItem>())
            {
                var type = InvertApplication.FindTypeByNameExternal(property.RelatedTypeName);
                if (type == null) continue;

                Ctx.TryAddNamespace(type.Namespace);
            }

            if (Ctx.IsDesignerFile)
            {
                Ctx.CurrentDeclaration.BaseTypes.Add(typeof(MonoBehaviour).ToCodeReference());
            }

            Ctx.AddIterator("ControllerProperty", node => node.Children.OfType<ElementNode>());
        }

    }

    [ForceBaseType(typeof(SystemLoader))]
    [RequiresNamespace("uFrame.IOC")]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.MVVM")]
    [RequiresNamespace("UniRx")]
    [RequiresNamespace("UnityEngine")]
    [RequiresNamespace("uFrame.MVVM.ViewModels")]
    public partial class SystemLoaderTemplate
    {
        [GenerateMethod(CallBase = true), Inside(TemplateLocation.Both)]
        public void Load()
        {
            Ctx.CurrentMethod.Attributes |= MemberAttributes.Override;

            if (!Ctx.IsDesignerFile)
                Ctx.CurrentMethod.invoke_base();

            if (!Ctx.IsDesignerFile) return;

            foreach (var item in Ctx.Data.Children.OfType<ElementNode>().Distinct())
            {
                Ctx._("Container.RegisterViewModelManager<{0}>(new ViewModelManager<{0}>())", item.Name.AsViewModel());
                Ctx._("Container.RegisterController<{0}>({0})", item.Name.AsController());
            }

            foreach (var item in Ctx.Data.Instances.Distinct())
            {
                Ctx._("Container.RegisterViewModel<{0}>({1}, \"{1}\")", item.SourceItem.Name.AsViewModel(), item.Name, item.Name);
            }
        }

        [ForEach("Instances"), GenerateProperty]
        public virtual ViewModel _Name_
        {
            get
            {
                var instance = Ctx.ItemAs<InstancesReference>();
                Ctx.SetType(instance.SourceItem.Name.AsViewModel());

                Ctx.AddAttribute(typeof(InjectAttribute))
                   .Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(instance.Name)));

                Ctx._if("this.{0} == null", instance.Name.AsField())
                    .TrueStatements._("this.{0} = this.CreateViewModel<{1}>( \"{2}\")", instance.Name.AsField(),
                        instance.SourceItem.Name.AsViewModel(), instance.Name);

                Ctx.CurrentDeclaration._private_(Ctx.CurrentProperty.Type, instance.Name.AsField());
                Ctx._("return {0}", instance.Name.AsField());

                return null;
            }
        }

        [GenerateProperty(uFrameFormats.CONTROLLER_FORMAT)]
        public virtual Controller ControllerProperty
        {
            get
            {
                Ctx.SetType(Ctx.Item.Name.AsController());
                Ctx.AddAttribute(typeof(InjectAttribute));
                Ctx.CurrentDeclaration._private_(Ctx.CurrentProperty.Type, Ctx.Item.Name.AsController().AsField());
                Ctx.LazyGet(Ctx.Item.Name.AsController().AsField(), "Container.CreateInstance(typeof({0})) as {0};",
                    Ctx.Item.Name.AsController());
                return null;
            }
            set { Ctx._("{0} = value", Ctx.Item.Name.AsController().AsField()); }
        }
    }
}
