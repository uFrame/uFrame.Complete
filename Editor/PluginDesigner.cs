using uFrame.Editor;
using uFrame.Editor.Attributes;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Compiling.CommonNodes;
using uFrame.Editor.Core;
using uFrame.Editor.Documentation;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Workspaces.Data;
using uFrame.IOC;
using uFrame.Editor.GraphUI;
using uFrame.Editor.GraphUI.Drawers;
using uFrame.Editor.Workspaces;

namespace uFrame.Architect.Editor
{
    using Data;
    using Generators;


    [UnityEditor.InitializeOnLoad]
    public class PluginDesigner : DiagramPlugin
    {
        static PluginDesigner()
        {
            InvertApplication.CachedAssembly(typeof(PluginDesigner).Assembly);
        }
        public override void Initialize(UFrameContainer container)
        {
#if DEBUG && UNITY_EDITOR
            //container.RegisterInstance<IToolbarCommand>(new PrintPlugins(), "Json");

#endif
            //container.RegisterInstance<IDiagramNodeCommand>(new SelectColorCommand(), "SelectColor");
            var pluginConfig = container
                .AddItem<ShellNodeSectionsSlot>()
                .AddItem<ShellNodeInputsSlot>()
                .AddItem<ShellNodeOutputsSlot>()
                .AddItem<TemplatePropertyReference>()
                .AddItem<TemplateMethodReference>()
                .AddItem<TemplateFieldReference>()
                .AddItem<TemplateEventReference>()
                .AddItem<ShellAcceptableReferenceType>()
                .AddItem<ShellConnectableReferenceType>()
                .AddTypeItem<ShellPropertySelectorItem>()
                .AddGraph<PluginGraphData, ShellPluginNode>("Shell Plugin")
                .Color(NodeColor.Green)

                .HasSubNode<IShellNode>()
                .HasSubNode<TypeReferenceNode>()
                .HasSubNode<ShellNodeConfig>()
                .HasSubNode<ScreenshotNode>()
#if UNITY_EDITOR
            // .AddCodeTemplate<DocumentationTemplate>()
#endif
            ;
            // container.AddNode<ScreenshotNode, ScreenshotNodeViewModel, ScreenshotNodeDrawer>("Screenshot");
            container.AddWorkspaceConfig<ArchitectWorkspace>("Architect", "Create a uFrame Architect workspace for creating plugin graphs.")
                .WithGraph<PluginGraphData>("Plugin", "Creates a new plugin graph for creating node configurations.");
            var shellConfigurationNode =
                container.AddNode<ShellNodeConfig, ShellNodeConfigViewModel, ShellNodeConfigDrawer>("Node Config")
                    .HasSubNode<ShellNodeConfig>()
                    .HasSubNode<ScreenshotNode>()
                    .HasSubNode<ShellTemplateConfigNode>()
                ;
            // shellConfigurationNode.AddFlag("Graph Type");

            container.AddNode<ShellTemplateConfigNode>("Code Template")
                .Color(NodeColor.Purple);


            RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellNodeConfig, ShellNodeConfigTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellNodeConfigSection, ShellNodeConfigReferenceSectionTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellNodeConfigSection, ShellNodeConfigChildItemTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellNodeConfig, ShellNodeAsGraphTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellPluginNode, ShellConfigPluginTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellNodeConfig, ShellNodeConfigViewModelTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellNodeConfig, ShellNodeConfigDrawerTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellTemplateConfigNode, ShellNodeConfigTemplateTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<IShellSlotType, ShellSlotItemTemplate>();
#if UNITY_EDITOR
            if (GenerateDocumentation)
            {
                RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellPluginNode, DocumentationTemplate>();
                RegisteredTemplateGeneratorsFactory.RegisterTemplate<IDocumentable, DocumentationPageTemplate>();
            }
#endif


            container.Connectable<ShellNodeConfigSection, ShellNodeConfig>();
            container.Connectable<ShellNodeConfigSection, ShellNodeConfigSection>();
            container.Connectable<IShellNodeConfigItem, IShellNodeConfigItem>();
            container.Connectable<ShellNodeConfigOutput, ShellNodeConfigInput>();
            container.Connectable<ShellNodeConfigOutput, ShellNodeConfig>();
            container.Connectable<ShellNodeConfigOutput, ShellNodeConfigSection>();
            container.Connectable<ShellNodeConfig, ShellNodeConfigInput>();
            container.Connectable<ShellNodeConfig, ShellNodeConfigSection>();
            container.Connectable<IShellNodeConfigItem, ShellTemplateConfigNode>();

            container.Connectable<ShellNodeConfigSection, ShellNodeConfigInput>();

            container.Connectable<ShellNodeConfigSection, ShellNodeConfigSection>();
        }

        public class ArchitectWorkspace : Workspace
        {

        }

        [InspectorProperty]
        public static bool GenerateDocumentation
        {
            get { return false; }
            //   InvertGraphEditor.Prefs.GetBool("PLUGINDESIGNER_GENDOCS", true); }
            set
            {
                //            InvertApplication.Prefs.SetBool("PLUGINDESIGNER_GENDOCS", value);
            }
        }
    }
}