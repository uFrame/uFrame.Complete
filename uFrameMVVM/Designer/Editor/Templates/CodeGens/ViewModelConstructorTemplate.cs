using System;
using uFrame.Kernel;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.DesignerFile, ClassNameFormat = uFrameFormats.VIEW_MODEL_FORMAT), AsPartial]
    public partial class ViewModelConstructorTemplate : IClassTemplate<ElementNode>, ITemplateCustomFilename
    {
        public TemplateContext<ElementNode> Ctx { get; set; }

        public string Filename
        {
            get
            {
                if (Ctx.Data.Name == null)
                {
                    throw new Exception(Ctx.Data.Name + " Graph name is empty");
                }
                return Path2.Combine(Ctx.Data.Graph.Name + "/ViewModels.designer", Ctx.Data.Name + "ViewModel.designer.cs");
            }
        }

        // Replace by ITemplateCustomFilename's Filename
        public string OutputPath { get { return ""; } }

        public bool CanGenerate { get { return true; } }

        public void TemplateSetup()
        {
            // Ensure the namespaces for each property type are property set up
            Ctx.CurrentDeclaration.BaseTypes.Clear();
            Ctx.CurrentDeclaration.Name = string.Format(uFrameFormats.VIEW_MODEL_FORMAT, Ctx.Data.Name);
        }
    }

    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.MVVM")]
    [RequiresNamespace("uFrame.Kernel.Serialization")]
    public partial class ViewModelConstructorTemplate
    {
        [GenerateConstructor("aggregator")]
        public void AggregatorConstructor(IEventAggregator aggregator)
        {

        }
    }
}
