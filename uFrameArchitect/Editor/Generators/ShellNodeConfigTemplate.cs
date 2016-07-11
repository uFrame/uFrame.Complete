using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;
using uFrame.Json;

namespace uFrame.Architect.Editor.Generators
{
    using Data;

    [TemplateClass(TemplateLocation.Both, ClassNameFormat = "{0}Node", AutoInherit = true)]
    public class ShellNodeConfigTemplate : GenericNode, IClassTemplate<ShellNodeConfig>
    {
        public TemplateContext<ShellNodeConfig> Ctx { get; set; }

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
            //Ctx.TryAddNamespace("Invert.Core");
            //Ctx.TryAddNamespace("Invert.Core.GraphDesigner");
            Ctx.TryAddNamespace("uFrame.Editor.Configurations");
            Ctx.TryAddNamespace("uFrame.Editor.Core");
            Ctx.TryAddNamespace("uFrame.Editor.Graphs.Data");
            var i = new CodeTypeDeclaration(Ctx.Data.ReferenceClassName)
            {
                IsInterface = true,
                Attributes = MemberAttributes.Public,
                IsPartial = true,
            };
            i.BaseTypes.Add(new CodeTypeReference(typeof(IDiagramNodeItem)));
            i.BaseTypes.Add(new CodeTypeReference(typeof(IConnectable)));
            Ctx.Namespace.Types.Add(i);
            if (Ctx.Data.Inheritable && Ctx.IsDesignerFile)
            {
                Ctx.CurrentDeclaration.BaseTypes.Clear();
                Ctx.CurrentDeclaration.BaseTypes.Add(typeof(GenericInheritableNode));
            }
            if (Ctx.IsDesignerFile && Ctx.Data.IsClass)
            {
                Ctx.CurrentDeclaration.BaseTypes.Add(typeof(IClassTypeNode));
            }

            Ctx.AddIterator("PossibleReferenceItems", _ => _.Sections.Where(p => p.SectionType == ShellNodeConfigSectionType.ReferenceItems));
            Ctx.AddIterator("ReferenceSectionItems", _ => _.Sections.Where(p => p.SectionType == ShellNodeConfigSectionType.ReferenceItems));
            Ctx.AddIterator("SectionItems", _ => _.Sections.Where(p => p.SectionType == ShellNodeConfigSectionType.ChildItems));
            Ctx.AddIterator("InputSlotId", _ => _.InputSlots);
            Ctx.AddIterator("InputSlot", _ => _.InputSlots);
            Ctx.AddIterator("OutputSlotId", _ => _.OutputSlots);
            Ctx.AddIterator("OutputSlot", _ => _.OutputSlots);
            Ctx.AddCondition("AllowMultipleOutputs", _ => !_.Inheritable);
            Ctx.AddCondition("ClassName", _ => Ctx.Data.IsClass);
            //Ctx.AddIterator("CustomSelectorItems", _ => _.CustomSelectors);
            foreach (var item in Ctx.Data.IncludedInSections)
            {
                Ctx.CurrentDeclaration.BaseTypes.Add(item.ReferenceClassName);
            }

        }
        [GenerateProperty(TemplateLocation.DesignerFile)]
        public virtual string ClassName
        {
            get
            {
                Ctx._("return this.Name");
                return null;
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
                var referenceName = Ctx.ItemAs<ShellNodeConfigSection>().ReferenceClassName;
                Ctx.SetTypeArgument(typeof(IItem));
                Ctx._("return this.Repository.AllOf<{0}>().Cast<IItem>()", referenceName);
                //Ctx.AddAttribute(typeof (ReferenceSection), Ctx.Item.Name);
                return null;
            }
        }

        [GenerateProperty("{0}")]
        public virtual IEnumerable<GenericReferenceItem> SectionItems
        {
            get
            {
                var item = Ctx.ItemAs<ShellNodeConfigSection>();

                Ctx.SetTypeArgument(item.ClassName);

                var attribute = Ctx.AddAttribute(typeof(Section))
                    .AddArgument(new CodePrimitiveExpression(Ctx.Item.Name))
                    .AddArgument("SectionVisibility.{0}", item.Visibility.ToString())
                    ;
                attribute.Arguments.Add(new CodeAttributeArgument("OrderIndex", new CodePrimitiveExpression(item.Row)));
                attribute.Arguments.Add(new CodeAttributeArgument("IsNewRow", new CodePrimitiveExpression(item.IsNewRow)));

                Ctx._("return PersistedItems.OfType<{0}>()", item.ClassName);
                return null;
            }
        }

        [GenerateProperty("{0}")]
        public virtual IEnumerable<GenericReferenceItem> ReferenceSectionItems
        {
            get
            {
                var referenceSection = Ctx.ItemAs<ShellNodeConfigSection>();

                Ctx.SetTypeArgument(referenceSection.ClassName);
                var attributes = Ctx.AddAttribute(typeof(ReferenceSection))
                    .AddArgument(new CodePrimitiveExpression(Ctx.Item.Name))
                    .AddArgument("SectionVisibility.{0}", referenceSection.Visibility.ToString())
                    .AddArgument(new CodePrimitiveExpression(referenceSection.AllowDuplicates))
                    .AddArgument(new CodePrimitiveExpression(referenceSection.IsAutomatic))
                    .AddArgument(string.Format("typeof({0})", referenceSection.ReferenceClassName))
                    .AddArgument(new CodePrimitiveExpression(referenceSection.IsEditable))
                    .AddArgument("OrderIndex", new CodePrimitiveExpression(referenceSection.Row))
                    .AddArgument("HasPredefinedOptions", new CodePrimitiveExpression(referenceSection.HasPredefinedOptions))
                    .AddArgument("IsNewRow", new CodePrimitiveExpression(referenceSection.IsNewRow))
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
        [GenerateProperty, WithLazyField(DefaultExpression = "Guid.NewGuid().ToString()"), WithNameFormat("{0}InputSlotId"), WithAttributes(typeof(JsonProperty))]
        public virtual string InputSlotId { get; set; }

        [GenerateProperty("{0}InputSlot")]
        public virtual GenericReferenceItem InputSlot
        {
            get
            {

                var item = Ctx.ItemAs<ShellNodeConfigInput>();
                var field = Ctx.CurrentDeclaration._private_(item.ClassName, "_" + item.Name.Clean());

                Ctx.SetType(item.ClassName);
                var attribute = Ctx.AddAttribute(typeof(InputSlot))
                    .AddArgument(new CodePrimitiveExpression(item.Name))
                    .AddArgument(new CodePrimitiveExpression(item.AllowMultiple))
                    .AddArgument("SectionVisibility.{0}", item.Visibility.ToString())

                    ;
                attribute.Arguments.Add(new CodeAttributeArgument("OrderIndex", new CodePrimitiveExpression(item.Row)));
                attribute.Arguments.Add(new CodeAttributeArgument("IsNewRow", new CodePrimitiveExpression(item.IsNewRow)));

                Ctx._if("Repository == null", field.Name).TrueStatements._("return null");
                Ctx._if("{0} != null", field.Name).TrueStatements._("return {0}", field.Name);

                Ctx._("return {0} ?? ({0} = new {1}() {{ Repository = Repository, Node = this, Identifier = {2}InputSlotId }})", field.Name, item.ClassName, Ctx.Item.Name.Clean());
                //Ctx._("{0}.Node = this", field.Name);

                //Ctx._("return {0}", field.Name);
                return null;
            }
            //set { Ctx._("if (value != null) {0}InputSlotId = value.Identifier", Ctx.Item.Name.Clean()); }

        }

        [GenerateProperty, WithLazyField(DefaultExpression = "Guid.NewGuid().ToString()"), WithNameFormat("{0}OutputSlotId"), WithAttributes(typeof(JsonProperty))]
        public virtual string OutputSlotId { get; set; }

        [GenerateProperty("{0}OutputSlot")]
        public virtual GenericReferenceItem OutputSlot
        {
            get
            {
                var item = Ctx.ItemAs<ShellNodeConfigOutput>();
                var field = Ctx.CurrentDeclaration._private_(item.ClassName, "_" + item.Name.Clean());
                Ctx.SetType(item.ClassName);
                var attribute = Ctx.AddAttribute(typeof(OutputSlot))
                    .AddArgument(new CodePrimitiveExpression(item.Name))
                    .AddArgument(new CodePrimitiveExpression(item.AllowMultiple))
                    .AddArgument("SectionVisibility.{0}", item.Visibility.ToString())
                    ;
                attribute.Arguments.Add(new CodeAttributeArgument("OrderIndex", new CodePrimitiveExpression(item.Row)));
                attribute.Arguments.Add(new CodeAttributeArgument("IsNewRow", new CodePrimitiveExpression(item.IsNewRow)));

                Ctx._if("Repository == null", field.Name).TrueStatements._("return null");
                Ctx._if("{0} != null", field.Name).TrueStatements._("return {0}", field.Name);

                Ctx._("return {0} ?? ({0} = new {1}() {{ Repository = Repository, Node = this, Identifier = {2}OutputSlotId }})", field.Name, item.ClassName, Ctx.Item.Name.Clean());
                return null;
            }
            //set { Ctx._("if (value != null) {0}OutputSlotId = value.Identifier", Ctx.Item.Name.Clean()); }
        }

        //[GenerateMethod]
        //public override void RecordRemoved(IDataRecord record)
        //{
        //    foreach (var item in Ctx.Data.InputSlots)
        //    {
        //        Ctx._if("record.Identifier = {0}InputSlotId", item.Name )
        //            .TrueStatements._("Repository.Remove(this)");
        //    }
        //    foreach (var item in Ctx.Data.OutputSlots)
        //    {
        //        Ctx._if("record.Identifier = {0}OutputSlotId", item.Name)
        //            .TrueStatements._("Repository.Remove(this)");
        //    }
        //}

    }
}