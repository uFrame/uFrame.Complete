using System.Collections.Generic;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Architect.Editor.Data;
using uFrame.Editor.Configurations;
using uFrame.Editor.Documentation;

namespace uFrame.Architect.Editor
{
    [TemplateClass(TemplateLocation.Both, "{0}DocumentationProvider")]
    public class DocumentationTemplate : DocumentationDefaultProvider, IClassTemplate<ShellPluginNode>
    {
        [GenerateMethod(TemplateLocation.Both)]
        public override void GetPages(List<DocumentationPage> rootPages)
        {
            //if (Ctx.IsDesignerFile)
            //foreach (var item in Ctx.Data.Graph.Project.NodeItems.OfType<ShellNodeConfig>())
            //{
            //    //Ctx._("DocumentationPage {0}Page = null;",item.Name.Clean());
            //    Ctx._("rootPages.Add({0}Page)",item.Name.Clean());



            //}

        }
        //[TemplateProperty(TemplateLocation.DesignerFile)]
        //public object DocumentNodeProperty
        //{
        //    get
        //    {
        //        this.Ctx.CurrentProperty.Name = Ctx.Item.Name.Clean() + "Page";
        //        var field = Ctx.CurrentDecleration._private_(typeof(DocumentationPage),
        //            "_" + this.Ctx.CurrentProperty.Name);

        //        var item = Ctx.ItemAs<ShellNodeConfig>();

        //        Ctx.PushStatements(Ctx._if("{0} == null", field.Name)
        //            .TrueStatements);

        //        Ctx._("{0} = new DocumentationPage(\"{1}\", Document{1}Node, typeof({2})))", field.Name, item.Name, item.ClassName);
        //        foreach (var nodeItem in item.PersistedItems.OfType<ShellNodeConfigItem>())
        //        {
        //            Ctx._("{0}.ChildItems.Add({1})", field.Name, nodeItem.Node.Name + nodeItem.Name.Clean() + "Page");
        //        }

        //        Ctx.PopStatements();
        //        Ctx._("return {0}", field.Name);
        //        return null;
        //    }
        //}

        //[TemplateProperty(TemplateLocation.DesignerFile)]
        //public object DocumentItemProperty {
        //    get
        //    {
        //        this.Ctx.CurrentProperty.Name = Ctx.Item.Node.Name + Ctx.Item.Name.Clean() + "Page";
        //        var field = Ctx.CurrentDecleration._private_(typeof (DocumentationPage),
        //            "_" + this.Ctx.CurrentProperty.Name);

        //        var item = Ctx.ItemAs<IShellNodeConfigItem>();

        //        Ctx.PushStatements(Ctx._if("{0} == null",field.Name)
        //            .TrueStatements);
        //        Ctx._("{0} = new DocumentationPage(\"{3}\", Document{0}Node_{2}, typeof({1})))", field.Name, item.ClassName, item.TypeName.Clean(), item.Name);
        //        Ctx.PopStatements();
        //        Ctx._("return {0}", field.Name);
        //        return null;
        //    } }



        //[TemplateMethod(TemplateLocation.Both, 
        //    AutoFill = AutoFillType.NameOnly, 
        //    NameFormat = "Document{0}Node")]
        //public virtual void DocumentNode(IDocumentationBuilder builder)
        //{

        //    if (Ctx.IsDesignerFile)
        //    {
        //        Ctx._("builder.Title(\"{0}\")", Ctx.Item.Name);
        //        //Ctx._("builder.Paragraph(\"{0}\")", Ctx.ItemAs<DiagramNode>().Comments);
        //    }
        //}

        //[TemplateMethod(TemplateLocation.Both,
        //    AutoFill = AutoFillType.NameOnly,
        //    NameFormat = "Document{0}")]
        //public virtual void DocumentItem(IDocumentationBuilder builder)
        //{
        //    this.Ctx.CurrentMethod.Name = "Document" + Ctx.Item.Node.Name + "Node_" + Ctx.ItemAs<IShellNodeConfigItem>().TypeName.Clean();
        //    if (Ctx.IsDesignerFile)
        //    {
        //        Ctx._("builder.Title(\"{0}\")", Ctx.Item.Name);
        //    }
        //}

        public string OutputPath
        {
            get { return Path2.Combine("Editor", "Documentation"); }
        }
        public bool CanGenerate { get { return true; } }
        public void TemplateSetup()
        {
            Ctx.TryAddNamespace("Invert.Core.GraphDesigner");
            //Ctx.AddIterator("DocumentNodeProperty", _ => _.Graph.Project.NodeItems.OfType<ShellNodeConfig>().Distinct());
            //Ctx.AddIterator("DocumentItemProperty", _ => _.Graph.Project.AllGraphItems.OfType<IShellNodeConfigItem>().Distinct());

            //Ctx.AddIterator("DocumentNode", _ => _.Graph.Project.NodeItems.OfType<ShellNodeConfig>().Distinct());
            //Ctx.AddIterator("DocumentItem", _ => _.Graph.Project.AllGraphItems.OfType<IShellNodeConfigItem>().Distinct());
        }

        public TemplateContext<ShellPluginNode> Ctx { get; set; }

    }
}