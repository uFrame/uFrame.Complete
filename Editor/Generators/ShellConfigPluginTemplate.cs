using System.CodeDom;
using System.Linq;
using uFrame.Architect.Editor.Data;
using uFrame.Editor;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.TypesSystem;
using uFrame.IOC;

namespace uFrame.Architect.Editor.Generators
{
    [TemplateClass(TemplateLocation.Both, ClassNameFormat = "{0}")]
    public class ShellConfigPluginTemplate : DiagramPlugin, IClassTemplate<ShellPluginNode>
    {
        #region Template Setup

        public string OutputPath
        {
            get { return Path2.Combine("Editor", "Plugins"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            Ctx.AddIterator("NodeConfigProperty", _ => _.Graph.NodeItems.OfType<ShellNodeConfig>());
            Ctx.AddIterator("GetSelectionCommand", _ => _.Graph.AllGraphItems.OfType<ShellNodeConfigSection>().Where(x => x.IsTyped && x.SectionType == ShellNodeConfigSectionType.ChildItems));
            //Ctx.TryAddNamespace("Invert.Core");
            //Ctx.TryAddNamespace("Invert.Core.GraphDesigner");
            Ctx.TryAddNamespace("uFrame.Editor");
            Ctx.TryAddNamespace("uFrame.Editor.Configurations");
            Ctx.TryAddNamespace("uFrame.Editor.Graphs.Data");
            Ctx.TryAddNamespace("uFrame.Editor.GraphUI");
            Ctx.TryAddNamespace("uFrame.Editor.GraphUI.Drawers");
            Ctx.TryAddNamespace("uFrame.Editor.TypesSystem");
            Ctx.TryAddNamespace("uFrame.IOC");
        }

        [GenerateMethod("Get{0}SelectionCommand", TemplateLocation.Both, true)]
        public virtual SelectTypeCommand GetSelectionCommand()
        {
            Ctx._("return new SelectTypeCommand() {{ IncludePrimitives = true, AllowNone = false }}");
            return null;
        }


        public TemplateContext<ShellPluginNode> Ctx { get; set; }
        #endregion

        public override bool Ignore
        {
            get
            {
                return true;
            }
        }

        [GenerateProperty("{0}"), WithField]
        public NodeConfig<GenericNode> NodeConfigProperty
        {
            get
            {
                var item = Ctx.ItemAs<IClassTypeNode>().ClassName;
                Ctx.SetTypeArgument(item);
                return null;
            }
            set
            {

            }
        }

        [GenerateMethod(TemplateLocation.Both, true)]
        public override void Initialize(UFrameContainer container)
        {
            if (!Ctx.IsDesignerFile) return;
            Ctx.CurrentMethodAttribute.CallBase = false;
            var method = Ctx.CurrentMethod;

            //foreach (var plugin in Ctx.Data.Repository.AllOf<ShellPluginNode>())
            //{
            //    //foreach (var item in Ctx.Data.Project.NodeItems.OfType<ShellNodeConfig>().Where(p => p.IsGraphType))
            //    //{
            //        Ctx._("container.RegisterInstance<IDocumentationProvider>(new {0}DocumentationProvider(), \"{0}\")",plugin.Name);
            //    //}
            //}

            //foreach (var item in Ctx.Data.Graph.NodeItems.OfType<ShellChildItemTypeNode>())
            //{
            //    if (!item["Typed"]) continue;


            //}
            foreach (var itemType in Ctx.Data.Repository.AllOf<ShellNodeConfigSection>().Where(p => p.IsValid && p.SectionType == ShellNodeConfigSectionType.ChildItems || p.SectionType == ShellNodeConfigSectionType.ReferenceItems))
            {

                if (itemType.IsTyped)
                {
                    //if (itemType.SectionType == ShellNodeConfigSectionType.ChildItems)
                    //method._("container.RegisterInstance<IEditorCommand>(Get{0}SelectionCommand(), typeof({1}).Name + \"TypeSelection\");", itemType.Name, itemType.ClassName);

                    //if (itemType.IsCustom)
                    //{
                    //    method.Statements.Add(
                    //        new CodeSnippetExpression(string.Format("container.AddTypeItem<{0},{1}ViewModel,{1}Drawer>()", itemType.ClassName, itemType.Name)));
                    //}
                    //else
                    //{
                    method.Statements.Add(
                        new CodeSnippetExpression(string.Format("container.AddTypeItem<{0}>()", itemType.ClassName)));
                    //}

                }
                else
                {
                    //if (itemType.Flags.ContainsKey("Custom") && itemType.Flags["Custom"])
                    //{
                    //    method.Statements.Add(
                    //    new CodeSnippetExpression(string.Format("container.AddItem<{0}, {1}ViewModel, {1}Drawer>()", itemType.ClassName, itemType.Name)));
                    //}
                    //else
                    //{
                    method.Statements.Add(
                        new CodeSnippetExpression(string.Format("container.AddItem<{0}>()", itemType.ClassName)));
                    //}
                }
            }

            //var graphTypes = Ctx.Data.Graph.NodeItems.OfType<ShellNodeConfig>().Where(p => p.IsValid && p.IsGraphType).ToArray();
            foreach (var nodeType in Ctx.Data.Repository.AllOf<ShellNodeConfig>().Where(p => p.IsValid))
            {
                InitializeNodeType(method, nodeType);

            }

            foreach (var item in Ctx.Data.Repository.AllOf<IShellNodeConfigItem>())
            {
                var connectableTo = item.OutputsTo<IShellNodeConfigItem>();
                foreach (var c in connectableTo)
                {
                    method._("container.Connectable<{0},{1}>()", item.ClassName, c.ClassName);
                }
            }


            foreach (var item in Ctx.Data.Repository.AllOf<IShellNodeConfigItem>())
            {
                foreach (var template in item.OutputsTo<ShellTemplateConfigNode>())
                {
                    method.Statements.Add(new CodeSnippetExpression(string.Format("RegisteredTemplateGeneratorsFactory.RegisterTemplate<{0},{1}>()", item.ClassName, template.Name)));
                }
            }
        }

        private static void InitializeNodeType(CodeMemberMethod method, ShellNodeConfig nodeType)
        {
            var varName = nodeType.Name;

            if (nodeType.IsGraphType)
            {
                method.Statements.Add(
                    new CodeSnippetExpression(string.Format("{1} = container.AddGraph<{0}, {2}>(\"{0}\")",
                        nodeType.Name + "Graph", varName, nodeType.ClassName)));
            }
            else
            {

                method.Statements.Add(
                    new CodeSnippetExpression(string.Format("{1} = container.AddNode<{0},{0}ViewModel,{0}Drawer>(\"{2}\")", nodeType.ClassName, varName, nodeType.NodeLabel)));

            }



            if (nodeType.Inheritable)
            {
                method.Statements.Add(new CodeSnippetExpression(string.Format("{0}.Inheritable()", varName)));
            }

            method.Statements.Add(
                new CodeSnippetExpression(string.Format("{0}.Color(NodeColor.{1})", varName, nodeType.Color.ToString())));



            foreach (var item in nodeType.SubNodes)
            {
                method.Statements.Add(
                    new CodeSnippetExpression(string.Format("{0}.HasSubNode<{1}Node>()", varName, item.Name)));
            }

        }

    }
}