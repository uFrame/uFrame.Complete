using System.CodeDom;
using System.Linq;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Architect.Editor.Data;
using uFrame.Editor;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.TypesSystem;
using uFrame.IOC;

namespace uFrame.Architect.Editor.Generators
{
    [TemplateClass(TemplateLocation.Both, ClassNameFormat = "{0}")]
    public class ShellPluginTemplate : DiagramPlugin, IClassTemplate<ShellPluginNode>
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
            Ctx.AddIterator("NodeConfigProperty", _ => _.Graph.NodeItems.OfType<ShellNodeTypeNode>());
            Ctx.AddIterator("GetSelectionCommand", _ => _.Graph.NodeItems.OfType<ShellChildItemTypeNode>().Where(x => x["Typed"]));
            //Ctx.TryAddNamespace("Invert.Core");
            //Ctx.TryAddNamespace("Invert.Core.GraphDesigner");
            Ctx.TryAddNamespace("uFrame.Editor.Configurations");
            Ctx.TryAddNamespace("uFrame.Editor.Core");
            Ctx.TryAddNamespace("uFrame.Editor.Graphs.Data");
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
            //foreach (var item in Ctx.Data.Graph.NodeItems.OfType<ShellChildItemTypeNode>())
            //{
            //    if (!item["Typed"]) continue;
            //    method._(
            //        "container.RegisterInstance<IEditorCommand>(Get{0}SelectionCommand(), typeof({1}).Name + \"TypeSelection\");", item.Name, item.ClassName);

            //}
            foreach (var itemType in Ctx.Data.Graph.NodeItems.OfType<IShellNode>().Where(p => p.IsValid))
            {
                if (itemType is ShellNodeTypeNode) continue;
                if (itemType is ShellSectionNode) continue;
                if (itemType is ShellGraphTypeNode) continue;
                if (itemType["Typed"])
                {
                    method.Statements.Add(
                        new CodeSnippetExpression(string.Format("container.AddTypeItem<{0},{1}ViewModel,{1}Drawer>()", itemType.ClassName, itemType.Name)));
                }
                else
                {
                    //if (itemType.Flags.ContainsKey("Custom") && itemType["Custom"])
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
            var graphTypes = Ctx.Data.Graph.NodeItems.OfType<ShellGraphTypeNode>().Where(p => p.IsValid).ToArray();
            foreach (var nodeType in Ctx.Data.Graph.NodeItems.OfType<ShellNodeTypeNode>().Where(p => p.IsValid))
            {
                InitializeNodeType(method, nodeType, graphTypes.FirstOrDefault(p => p.RootNode == nodeType));
            }

            foreach (var nodeType in Ctx.Data.Graph.NodeItems.OfType<IShellConnectable>().Where(p => p.IsValid))
            {
                foreach (var item in nodeType.ConnectableTo)
                {
                    method._("container.Connectable<{0},{1}>()", nodeType.ClassName, item.SourceItem.ClassName);
                }

            }
            foreach (var nodeType in Ctx.Data.Graph.NodeItems.OfType<IReferenceNode>().Where(p => p.IsValid))
            {


                if (nodeType["Output"])
                {
                    method._("container.Connectable<{0},{1}>()", nodeType.ClassName, nodeType.ReferenceClassName);
                }
                else
                {
                    method._("container.Connectable<{0},{1}>()", nodeType.ReferenceClassName, nodeType.ClassName);
                }
            }
        }

        private static void InitializeNodeType(CodeMemberMethod method, ShellNodeTypeNode nodeType, ShellGraphTypeNode graphType)
        {
            var varName = nodeType.Name;
            var type = graphType == null ? "Node" : "Graph";
            if (graphType != null)
            {
                method.Statements.Add(
                    new CodeSnippetExpression(string.Format("{1} = container.AddGraph<{0}, {2}>(\"{0}\")",
                        graphType.ClassName, varName, graphType.RootNode.ClassName)));
            }
            else
            {

                method.Statements.Add(
                    new CodeSnippetExpression(string.Format("{1} = container.Add{2}<{0}Node,{0}NodeViewModel,{0}NodeDrawer>(\"{0}\")", nodeType.Name, varName, type)));

            }


            if (nodeType["Inheritable"])
            {
                method.Statements.Add(new CodeSnippetExpression(string.Format("{0}.Inheritable()", varName)));
            }



            foreach (var item in nodeType.SubNodes)
            {
                method.Statements.Add(
                    new CodeSnippetExpression(string.Format("{0}.HasSubNode<{1}Node>()", varName, item.Name)));
            }

        }

    }
}