using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Architect.Editor.Data;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Architect.Editor.Generators
{
    [TemplateClass(TemplateLocation.Both, ClassNameFormat = "{0}Node")]
    public class ShellNodeTypeTemplate : GenericNode, IClassTemplate<ShellNodeTypeNode>
    {
        public TemplateContext<ShellNodeTypeNode> Ctx { get; set; }

        public string OutputPath
        {
            get { return Path2.Combine("Editor", "Nodes"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            //Ctx.TryAddNamespace("Invert.Core.GraphDesigner");
            Ctx.TryAddNamespace("uFrame.Editor.Configurations");
            Ctx.TryAddNamespace("uFrame.Editor.Core");
            Ctx.TryAddNamespace("uFrame.Editor.Graphs.Data");
            if (Ctx.Data.Inheritable && Ctx.IsDesignerFile)
            {
                Ctx.CurrentDeclaration.BaseTypes.Clear();
                Ctx.CurrentDeclaration.BaseTypes.Add(typeof(GenericInheritableNode));
            }
            Ctx.AddIterator("PossibleReferenceItems", _ => _.Sections.Where(p => p.SourceItem is ShellNodeTypeReferenceSection));
            Ctx.AddIterator("ReferenceSectionItems", _ => _.Sections.Where(p => p.SourceItem is ShellNodeTypeReferenceSection));
            Ctx.AddIterator("SectionItems", _ => _.Sections.Where(p => p.SourceItem is ShellSectionNode));
            Ctx.AddIterator("InputSlot", _ => _.InputSlots);
            Ctx.AddIterator("OutputSlot", _ => _.OutputSlots);
            Ctx.AddCondition("AllowMultipleOutputs", _ => !_.Inheritable);
            //Ctx.AddIterator("CustomSelectorItems", _ => _.CustomSelectors);
            foreach (var item in Ctx.Data.IncludedInSections)
            {

                Ctx.CurrentDeclaration.BaseTypes.Add(item.ReferenceClassName);
            }

        }

        [GenerateProperty(TemplateLocation.DesignerFile)]
        public override bool AllowMultipleInputs
        {
            get
            {
                Ctx._("return {0}", Ctx.Data.AllowMultipleInputs ? "true" : "false");
                return base.AllowMultipleInputs;
            }
        }

        [GenerateProperty(TemplateLocation.DesignerFile)]
        public override bool AllowMultipleOutputs
        {
            get
            {
                Ctx._("return {0}", Ctx.Data.AllowMultipleOutputs ? "true" : "false");
                return base.AllowMultipleOutputs;
            }
        }

        [GenerateProperty("Possible{0}")]
        public virtual IEnumerable<GenericReferenceItem> PossibleReferenceItems
        {
            get
            {
                var referenceName = Ctx.ItemAs<ShellNodeSectionsSlot>().SourceItem.ReferenceClassName;
                Ctx.SetTypeArgument(referenceName);
                Ctx._("return this.Repository.AllOf<{0}>()", referenceName);
                //Ctx.AddAttribute(typeof (ReferenceSection), Ctx.Item.Name);
                return null;
            }
        }

        [GenerateProperty("{0}")]
        public virtual IEnumerable<GenericReferenceItem> SectionItems
        {
            get
            {
                var item = Ctx.ItemAs<ShellNodeSectionsSlot>();
                var section = item.SourceItem as ShellSectionNode;
                Ctx.SetTypeArgument(section.ReferenceType.ClassName);

                Ctx.AddAttribute(typeof(Section))
                    .AddArgument(new CodePrimitiveExpression(Ctx.Item.Name))
                    .AddArgument("SectionVisibility.{0}", section.Visibility.ToString())
                    .Arguments.Add(new CodeAttributeArgument("OrderIndex", new CodePrimitiveExpression(item.Order)))
                    ;

                Ctx._("return PersistedItems.OfType<{0}>()", section.ReferenceType.ClassName);
                return null;
            }
        }
        [GenerateProperty("{0}")]
        public virtual IEnumerable<GenericReferenceItem> ReferenceSectionItems
        {
            get
            {
                var slot = Ctx.ItemAs<ShellNodeSectionsSlot>();
                var referenceSection = slot.SourceItem as ShellNodeTypeReferenceSection;
                Ctx.SetTypeArgument(referenceSection.ClassName);
                var attributes = Ctx.AddAttribute(typeof(ReferenceSection))
                    .AddArgument(new CodePrimitiveExpression(Ctx.Item.Name))
                    .AddArgument("SectionVisibility.{0}", referenceSection.Visibility.ToString())
                    .AddArgument(new CodePrimitiveExpression(referenceSection.AllowDuplicates))
                    .AddArgument(new CodePrimitiveExpression(referenceSection.IsAutomatic))
                    .AddArgument(string.Format("typeof({0})", referenceSection.ReferenceClassName))
                    .AddArgument(new CodePrimitiveExpression(referenceSection.IsEditable))
                    .AddArgument("OrderIndex", new CodePrimitiveExpression(slot.Order))
                    .AddArgument("HasPredefinedOptions", new CodePrimitiveExpression(referenceSection.HasPredefinedOptions))
                    ;

                Ctx._("return PersistedItems.OfType<{0}>()", referenceSection.ClassName);
                return null;
            }
        }


        public virtual IEnumerable<GenericReferenceItem> InputItems
        {
            get { return null; }
        }

        public virtual IEnumerable<GenericReferenceItem> OutputItems
        {
            get { return null; }
        }

        //[TemplateProperty("{0}", AutoFillType.NameOnly, Location = TemplateLocation.Both)]
        //public virtual IEnumerable<GenericReferenceItem> CustomSelectorItems
        //{
        //    get
        //    {
        //        Ctx.SetTypeArgument(Ctx.ItemAs<ShellPropertySelectorItem>().ReferenceClassName);
        //        Ctx._("yield break");
        //        return null;
        //    }
        //}

        [GenerateProperty("{0}InputSlot")]
        public virtual GenericReferenceItem InputSlot
        {
            get
            {

                var item = Ctx.ItemAs<ShellNodeInputsSlot>();
                var field = Ctx.CurrentDeclaration._private_(item.SourceItem.ClassName, "_" + item.Name);

                Ctx.SetType(item.SourceItem.ClassName);
                Ctx.AddAttribute(typeof(InputSlot))
                    .AddArgument(new CodePrimitiveExpression(item.Name))
                    .AddArgument(new CodePrimitiveExpression(item.SourceItem.AllowMultiple))
                    .AddArgument("SectionVisibility.{0}", item.SourceItem.Visibility.ToString())
                    .Arguments.Add(new CodeAttributeArgument("OrderIndex", new CodePrimitiveExpression(item.Order)))
                    ;
                Ctx._if("{0} == null", field.Name)
                    .TrueStatements._("{0} = new {1}() {{  Repository = Repository, Node = this }}", field.Name);
                return null;
            }

        }

        [GenerateProperty("{0}OutputSlot")]
        public virtual GenericReferenceItem OutputSlot
        {
            get
            {
                var item = Ctx.ItemAs<ShellNodeOutputsSlot>();
                Ctx.SetType(item.SourceItem.ClassName);
                Ctx.AddAttribute(typeof(OutputSlot))
                    .AddArgument(new CodePrimitiveExpression(item.Name))
                    .AddArgument(new CodePrimitiveExpression(item.SourceItem.AllowMultiple))
                    .AddArgument("SectionVisibility.{0}", item.SourceItem.Visibility.ToString())
                    .Arguments.Add(new CodeAttributeArgument("OrderIndex", new CodePrimitiveExpression(item.Order)))
                    ;

                return null;
            }
        }

    }
}