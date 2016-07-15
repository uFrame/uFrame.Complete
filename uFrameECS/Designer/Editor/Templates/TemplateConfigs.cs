using System;
using System.CodeDom;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using uFrame.Attributes;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Kernel;
using UnityEditor;
using uFrame.Editor.Attributes;
using uFrame.ECS.Actions;
using uFrame.ECS.Editor;
using uFrame.ECS.APIs;
using uFrame.Editor;
using uFrame.ECS.Components;
using uFrame.ECS.UnityUtilities;
using uFrame.ECS.Systems;
using uFrame.Editor.Configurations;
using uFrame.Editor.Core;
using uFrame.Editor.Database.Data;
using uFrame.Editor.DebugSystem;
using uFrame.Editor.Graphs.Data;
using uFrame.IOC;

namespace uFrame.ECS.Templates
{
    [InitializeOnLoad]
    public class EcsTemplates : DiagramPlugin
    {
        static EcsTemplates()
        {
            InvertApplication.CachedAssembly(typeof(EcsTemplates).Assembly);
            InvertApplication.CachedAssembly(typeof(UFAction).Assembly);
            InvertApplication.CachedTypeAssembly(typeof(UFAction).Assembly);
        }
        public override void Initialize(UFrameContainer container)
        {
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<SystemNode, SystemTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<SystemNode, SystemPartialTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<ComponentNode, ComponentTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<ComponentNode, ComponentEditableTemplate>();
            //            RegisteredTemplateGeneratorsFactory.RegisterTemplate<ComponentGroupNode,ComponentGroupTemplate>();
            //          RegisteredTemplateGeneratorsFactory.RegisterTemplate<ComponentGroupNode,ComponentGroupManagerTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<EventNode, EventTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<GroupNode, GroupTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<GroupNode, GroupItemTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<HandlerNode, HandlerTemplate>();
            //RegisteredTemplateGeneratorsFactory.RegisterTemplate<HandlerNode, EditableHandlerTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<uFrameDatabaseConfig, DbLoaderTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<uFrameDatabaseConfig, EcsEditorExtensionTemplate>();
            //RegisteredTemplateGeneratorsFactory.RegisterTemplate<PropertyChangedNode, PropertyHandlerTemplate>();
            //            RegisteredTemplateGeneratorsFactory.RegisterTemplate<EntityNode, EntityTemplate>();

            RegisteredTemplateGeneratorsFactory.RegisterTemplate<CustomActionNode, CustomActionEditableTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<CustomActionNode, CustomActionDesignerTemplate>();      
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<CustomActionNode, CustomActionSequenceTemplate>();      
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<CodeActionNode, CodeActionEditableTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<CodeActionNode, CodeActionDesignerTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<SystemNode, LoaderTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<ModuleNode, ComponentExtensionsTemplate>();

            RegisteredTemplateGeneratorsFactory.RegisterTemplate<DescriptorNode, DescriptorInterfaceTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<DescriptorNode, DescriptorAttributeTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<DescriptorNode, DescriptorGroupTemplate>();

        }
    }

    [TemplateClass(TemplateLocation.DesignerFile, "I{0}"), AsPartial]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.ECS")]
    [RequiresNamespace("System.Linq")]
    [ForceBaseType(typeof(IEcsComponent))]
    [WithMetaInfo]
    public partial class DescriptorInterfaceTemplate : IClassTemplate<DescriptorNode>, ITemplateCustomFilename
    {
        public string Filename
        {
            get
            {
                return Path2.Combine("Descriptors", "I" + Ctx.Data.Name + ".cs");
            }
        }
        public string OutputPath { get { return string.Empty; } }
        public bool CanGenerate { get { return true; } }
        public void TemplateSetup()
        {
            this.Ctx.CurrentDeclaration.IsInterface = true;
            this.Ctx.SetBaseType(typeof(IEcsComponent));
        }

        public TemplateContext<DescriptorNode> Ctx { get; set; }

        [GenerateProperty]
        public PropertyInfo[] _Name_Members
        {
            get
            {
                //var field = Ctx.CurrentDeclaration._private_(typeof (PropertyInfo[]), "_" + Ctx.Data.Name);
                //Ctx._if("{0} == null", field.Name)
                //    .TrueStatements._("{0} = this.GetDescriptorProperties<{0}Attribute>().ToArray()", field.Name);
                //Ctx._("return {0}", field.Name);
                return null;
            }
        }
    }
    [TemplateClass(TemplateLocation.DesignerFile, "{0}Attribute"), AsPartial]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.ECS")]
    [ForceBaseType(typeof(Attribute))]
    [WithMetaInfo]
    public partial class DescriptorAttributeTemplate : IClassTemplate<DescriptorNode>, ITemplateCustomFilename
    {
        public string Filename
        {
            get
            {
                return Path2.Combine("Descriptors", Ctx.Data.Name + "Attribute.cs");
            }
        }
        public string OutputPath { get { return string.Empty; } }
        public bool CanGenerate { get { return Ctx.Data.Type == DescriptorNodeType.Properties; } }
        public void TemplateSetup()
        {
            this.Ctx.SetBaseType(typeof(Attribute));
        }

        public TemplateContext<DescriptorNode> Ctx { get; set; }
    }
    [TemplateClass(TemplateLocation.DesignerFile, "{0}Group"), AsPartial]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.ECS")]
    [WithMetaInfo]
    public partial class DescriptorGroupTemplate : IClassTemplate<DescriptorNode>, ITemplateCustomFilename
    {
        public string Filename
        {
            get
            {
                return Path2.Combine("Descriptors", Ctx.Data.Name + "Group.cs");
            }
        }
        public string OutputPath { get { return string.Empty; } }
        public bool CanGenerate { get { return true; } }
        public void TemplateSetup()
        {
            this.Ctx.SetBaseType("DescriptorGroup<I{0}>",Ctx.Data.Name);
        }

        public TemplateContext<DescriptorNode> Ctx { get; set; }
    }
    //[TemplateClass(TemplateLocation.DesignerFile, "{0}Attribute"), AsPartial]
    //[RequiresNamespace("uFrame.Kernel")]
    //[RequiresNamespace("uFrame.ECS")]
    //[ForceBaseType(typeof(Attribute))]
    //[WithMetaInfo]
    //public partial class DescriptorInterfaceTemplate : IClassTemplate<DescriptorNode>, ITemplateCustomFilename
    //{
    //    public string Filename
    //    {
    //        get
    //        {
    //            return Path2.Combine("Descriptors", "I" + Ctx.Data.Name + ".cs");
    //        }
    //    }
    //    public string OutputPath { get { return string.Empty; } }
    //    public bool CanGenerate { get { return true; } }
    //    public void TemplateSetup()
    //    {
    //        this.Ctx.CurrentDeclaration.IsInterface = true;
    //        this.Ctx.SetBaseType(typeof(IEcsComponent));
    //    }

    //    public TemplateContext<DescriptorNode> Ctx { get; set; }
    //}

    [TemplateClass(TemplateLocation.DesignerFile,"{0}Loader"), AsPartial]
    [RequiresNamespace("uFrame.ECS")]
    [RequiresNamespace("uFrame.ECS.UnityUtilities")]
    [RequiresNamespace("uFrame.Kernel")]
    [ForceBaseType(typeof(SystemLoader))]
    [WithMetaInfo]
    public partial class LoaderTemplate : IClassTemplate<SystemNode>, ITemplateCustomFilename
    {
        public string OutputPath
        {
            get { return Path2.Combine("Modules", Ctx.Data.Name, Ctx.Data.Name); }
        }

        public string Filename
        {
            get
            {
                return Path2.Combine("Systems", Ctx.Data.Name + "Loader.cs");
            }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            this.Ctx.CurrentDeclaration.Name = Ctx.Data.Name + "Loader";
        }

        public TemplateContext<SystemNode> Ctx { get; set; }
        public IEnumerable<GroupNode> Groups
        {
            get
            {
                return Ctx.Data.Graph.NodeItems.OfType<ISystemGroupProvider>().SelectMany(p => p.GetSystemGroups()).OfType<GroupNode>();
            }
        }

        [GenerateMethod, AsOverride]
        public void Load()
        {
            //Ctx._("EcsSystem system = null");
            Ctx._("this.AddSystem<{0}>()", Ctx.Data.Name);
         
        }


    }

    [TemplateClass(TemplateLocation.DesignerFile, "{0}ComponentExtensions")]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.ECS")]
    public partial class ComponentExtensionsTemplate : IClassTemplate<ModuleNode>, ITemplateCustomFilename
    {
        public string OutputPath
        {
            get { return Path2.Combine("Modules", Ctx.Data.Name, Ctx.Data.Name); }
        }

        public string Filename
        {
            get
            {
                return Path2.Combine("Systems", Ctx.Data.Name + "Extensions.cs");
            }
        }

        public bool CanGenerate
        {
            get { return Groups.Any() || Components.Any(); }
        }

        public void TemplateSetup()
        {
            
        }
        [TemplateComplete]
        public void Post()
        {
            this.Ctx.CurrentDeclaration.Name = Ctx.Data.Name + "Extensions";
            this.Ctx.CurrentDeclaration.BaseTypes.Clear();
            this.Ctx.CurrentDeclaration.MakeStatic();
        }
        public TemplateContext<ModuleNode> Ctx { get; set; }
        public IEnumerable<GroupNode> Groups
        {
            get
            {
                return Ctx.Data.Graph.NodeItems.OfType<GroupNode>();
            }
        }

        public IEnumerable<ComponentNode> Components
        {
            get
            {
                return Ctx.Data.Graph.NodeItems.OfType<ComponentNode>();
            }
        }

        public IEnumerable<IDiagramNode> All
        {
            get
            {
                return Components.OfType<IDiagramNode>().Concat(Groups.OfType<IDiagramNode>());
            }
        }

        [ForEach("All"), GenerateMethod]
        public void _Name_Manager()
        {
            this.Ctx.CurrentMethod.Parameters.Add(
                new CodeParameterDeclarationExpression("this uFrame.ECS.APIs.IEcsSystem", "system"));
            this.Ctx.CurrentMethod.MakeStatic();
            this.Ctx.CurrentMethod.ReturnType = string.Format("uFrame.ECS.APIs.IEcsComponentManagerOf<{0}>", Ctx.Item.Name).ToCodeReference();
            Ctx._("return system.ComponentSystem.RegisterComponent<{0}>()",Ctx.Item.Name);
        }

        [ForEach("All"), GenerateMethod]
        public void _Name_Components()
        {
            this.Ctx.CurrentMethod.Parameters.Add(
                new CodeParameterDeclarationExpression("this uFrame.ECS.APIs.IEcsSystem", "system"));
            this.Ctx.CurrentMethod.MakeStatic();
            this.Ctx.CurrentMethod.ReturnType = string.Format("List<{0}>", Ctx.Item.Name).ToCodeReference();
            Ctx._("return system.ComponentSystem.RegisterComponent<{0}>().Components", Ctx.Item.Name);
        }


  
    }

    [TemplateClass(TemplateLocation.DesignerFile)]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.ECS")]
    [WithMetaInfo]
    public partial class EcsEditorExtensionTemplate : IClassTemplate<uFrameDatabaseConfig>, ITemplateCustomFilename
    {
        public string OutputPath
        {
            get { return Path2.Combine("Editor", Ctx.Data.Title); }
        }

        public string Filename
        {
            get
            {
                return Path2.Combine("Editor", Ctx.Data.Title + "EditorExtensions.cs");
            }
        }

        public bool CanGenerate
        {
            get { return Ctx.Data.Repository.All<SystemNode>().Any(); }
        }

        public void TemplateSetup()
        {
            this.Ctx.CurrentDeclaration.Name = Ctx.Data.Title + "EditorExtensions";
            this.Ctx.CurrentDeclaration.BaseTypes.Clear();
            this.Ctx.CurrentDeclaration.Attributes |= MemberAttributes.Static;
            this.Ctx.CurrentDeclaration.CustomAttributes.Add(
                new CodeAttributeDeclaration(new CodeTypeReference(typeof (InitializeOnLoadAttribute))));

        }

        public TemplateContext<uFrameDatabaseConfig> Ctx { get; set; }

        [GenerateMethod]
        public void AddKernel()
        {
            var codeAttributeDeclaration = new CodeAttributeDeclaration(new CodeTypeReference(typeof(MenuItem)));
            
            codeAttributeDeclaration.Arguments.Add(
                new CodeAttributeArgument(new CodePrimitiveExpression(string.Format("GameObject/Create {0} Kernel", Ctx.Data.Title))));
            codeAttributeDeclaration.Arguments.Add(
               new CodeAttributeArgument(new CodePrimitiveExpression(false)));
            codeAttributeDeclaration.Arguments.Add(
               new CodeAttributeArgument(new CodePrimitiveExpression(0)));
            this.Ctx.CurrentMethod.CustomAttributes.Add(codeAttributeDeclaration);
            this.Ctx.CurrentMethod.Attributes |= MemberAttributes.Static;
            Ctx._("uFrame.ECS.Templates.EcsEditorExtensionTemplate.AddEcsKernelWith<{0}.{1}>()", Ctx.Data.Namespace , Ctx.Data.Title + "Loader");
        }

        public static void AddEcsKernelWith<TLoaderType>() where TLoaderType : Component
        {
            GameObject obj = new GameObject("_Kernel");
            obj.AddComponent<uFrameKernel>();
            obj.AddComponent<EcsSystemLoader>();
            obj.AddComponent<TLoaderType>();

        }
    }


    [TemplateClass(TemplateLocation.DesignerFile, "{0}Loader"), AsPartial]
    [RequiresNamespace("uFrame.ECS")]
    [RequiresNamespace("uFrame.ECS.Systems")]
    [RequiresNamespace("uFrame.ECS.UnityUtilities")]
    [RequiresNamespace("uFrame.Kernel")]
    [ForceBaseType(typeof(SystemLoader))]
    public partial class DbLoaderTemplate : IClassTemplate<uFrameDatabaseConfig>, ITemplateCustomFilename
    {
        public string OutputPath
        {
            get { return Path2.Combine( Ctx.Data.Title + "Loader.cs"); }
        }

        public string Filename
        {
            get
            {
                return Path2.Combine( Ctx.Data.Title + "Loader.cs");
            }
        }

        public bool CanGenerate
        {
            get { return Systems.Any(); }
        }

        public void TemplateSetup()
        {
            this.Ctx.CurrentDeclaration.Name = Ctx.Data.Title + "Loader";
        }

        public TemplateContext<uFrameDatabaseConfig> Ctx { get; set; }
        public IEnumerable<SystemNode> Systems
        {
            get  { return Ctx.Data.Database.AllOf<SystemNode>(); }
        }

        [GenerateMethod, AsOverride]
        public void Load()
        {
            Ctx._("EcsSystem system = null");
            foreach (var system in Systems)
            {
                Ctx._("system = this.AddSystem<{0}>()", system.Name);
            }
           

        }

        

    }


    [TemplateClass(TemplateLocation.DesignerFile, "{0}Loader"), AsPartial]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.ECS")]
    [ForceBaseType(typeof(SystemLoader))]
    public partial class DbComponentEnumLoaderTemplate : IClassTemplate<uFrameDatabaseConfig>, ITemplateCustomFilename
    {
        public string OutputPath
        {
            get { return Path2.Combine(Ctx.Data.Title + "Loader.cs"); }
        }

        public string Filename
        {
            get
            {
                return Path2.Combine(Ctx.Data.Title + "Loader.cs");
            }
        }

        public bool CanGenerate
        {
            get { return Systems.Any(); }
        }

        public void TemplateSetup()
        {
            this.Ctx.CurrentDeclaration.Name = Ctx.Data.Title + "Components";
        }

        public TemplateContext<uFrameDatabaseConfig> Ctx { get; set; }
        public IEnumerable<SystemNode> Systems
        {
            get { return Ctx.Data.Database.AllOf<SystemNode>(); }
        }

        [GenerateMethod, AsOverride]
        public void Load()
        {
            

        }



    }

    [TemplateClass(TemplateLocation.DesignerFile)]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.ECS.UnityUtilities")]
    [RequiresNamespace("UnityEngine")]
    [NamespacesFromItems]
    public partial class SequenceTemplate<TType> : IClassTemplate<TType>,ITemplateCustomFilename where TType : ISequenceNode
    {
        public virtual string Filename
        {
             get { return Path2.Combine("Handlers", Ctx.Data.Name + "Handler.cs"); }
        }
        public string OutputPath
        {
            get { return Path2.Combine("Handlers", Ctx.Data.Name + "Handler.cs"); }
        }

        public virtual bool CanGenerate
        {
            get { return Ctx.Data.CanGenerate; }
        }

        public void TemplateSetup()
        {
            this.Ctx.CurrentDeclaration.BaseTypes.Clear();
            this.Ctx.CurrentDeclaration.Name = Ctx.Data.HandlerMethodName;
        }

        public TemplateContext<TType> Ctx { get; set; }


        [GenerateMethod]
        public virtual void Execute()
        {
            if (DebugSystem.IsDebugMode)
                this.Ctx.SetType(typeof(IEnumerator));

            var csharpVisitor = new CSharpSequenceVisitor()
            {
                _ = Ctx
            };
            Ctx.Data.Accept(csharpVisitor);

            if (DebugSystem.IsDebugMode)
                Ctx._("yield break");
           
        }

       
    }

    [TemplateClass(TemplateLocation.DesignerFile)]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("UnityEngine")]
    [RequiresNamespace("uFrame.ECS")]
    public partial class HandlerTemplate : SequenceTemplate<HandlerNode>
    {
        public override bool CanGenerate
        {
            get { return Ctx.Data.Children.Any(); }
        }
    }

    [TemplateClass(TemplateLocation.Both)]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("UnityEngine")]
    public partial class EditableHandlerTemplate : IClassTemplate<HandlerNode>, ITemplateCustomFilename
    {
        [GenerateProperty(TemplateLocation.DesignerFile), WithField]
        public object Event
        {
            get
            {

                this.Ctx.SetType(Ctx.Data.EventType);
                return null;
            }
            set
            {

            }
        }

        [GenerateProperty(TemplateLocation.DesignerFile), WithField]
        public EcsSystem System { get; set; }

        [TemplateSetup]
        public void SetName()
        {
            Ctx.CurrentDeclaration.IsPartial = true;
            Ctx.CurrentDeclaration.Name = Ctx.Data.HandlerMethodName;
            Ctx.CurrentDeclaration.BaseTypes.Clear();
            if (Ctx.IsDesignerFile)
            {
                foreach (var item in Ctx.Data.FilterInputs)
                {
                    var context = item.FilterNode;
                    if (context == null) continue;
                    CreateFilterProperty(item, context);
                }
                Ctx.Data.AddProperties(Ctx);
            }
         

        }

        private void CreateFilterProperty(IFilterInput input, IMappingsConnectable inputFilter)
        {
            Ctx.CurrentDeclaration._public_(inputFilter.ContextTypeName, input.HandlerPropertyName);

        }
        [GenerateMethod, Inside((TemplateLocation.EditableFile))]
        public void Execute()
        {
            
        }

        public string OutputPath
        {
            get { return null; }
        }

        public bool CanGenerate
        {
            get { return false; }
        }

        public void TemplateSetup()
        {
            
        }

        public TemplateContext<HandlerNode> Ctx { get; set; }

        public string Filename
        {
            get
            {
                if (Ctx.IsDesignerFile)
                {
                    return Path2.Combine("CustomHandlers", Ctx.Data.Name + ".designer.cs");
                }
                return Path2.Combine("CustomHandlers", Ctx.Data.Name + ".cs");
            }
        }
    }


    [TemplateClass(TemplateLocation.DesignerFile)]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("UnityEngine")]
    public partial class PropertyHandlerTemplate : SequenceTemplate<PropertyChangedNode>
    {

    }
    //[TemplateClass(TemplateLocation.Both,"{0}PrefabPool")]
    //[RequiresNamespace("uFrame.Kernel")]
    //[RequiresNamespace("UnityEngine")]
    //[RequiresNamespace("uFrame.ECS")]
    //[ForceBaseType(typeof(EntityPrefabPool)), AsPartial]
    //public partial class EntityTemplate : IClassTemplate<EntityNode>
    //{

    //    public string OutputPath
    //    {
    //        get { return Path2.Combine(Ctx.Data.Graph.Name, "Entities"); }
    //    }

    //    public bool CanGenerate
    //    {
    //        get { return true; }
    //    }

    //    public void TemplateSetup()
    //    {
    //        Ctx.CurrentDeclaration.Name = Ctx.Data.Name + "PrefabPool";
    //        if (!Ctx.IsDesignerFile)
    //        {
    //            Ctx.CurrentDeclaration.BaseTypes.Clear();
    //        }
    //        else
    //        {
    //            foreach (var item in Ctx.Data.Components)
    //            {
    //                Ctx.CurrentDeclaration.CustomAttributes.Add(
    //                    new CodeAttributeDeclaration(typeof(RequireComponent).ToCodeReference(),
    //                        new CodeAttributeArgument(new CodeSnippetExpression(string.Format("typeof({0})", item.Name)))));
    //            }
    //        }


    //    }

    //    public TemplateContext<EntityNode> Ctx { get; set; }
    //}
     

    [TemplateClass(TemplateLocation.Both), AsPartial]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.ECS.Systems")]
    [AutoNamespaces]
    [NamespacesFromItems]
    public partial class SystemTemplate : IClassTemplate<SystemNode>, ITemplateCustomFilename
    {
        public string Filename
        {
            get
            {
                if (Ctx.Data.Name == null)
                {
                    throw new Exception(Ctx.Data.Name + " Graph name is empty");
                }
                if (Ctx.IsDesignerFile)
                {
                    return Path2.Combine("Systems", Ctx.Data.Name + ".designer.cs");
                }
                return Path2.Combine("Systems", Ctx.Data.Name + ".cs");
            }
        }
        public string OutputPath
        {
            get { return Path2.Combine("Systems", Ctx.Data.Name + ".cs"); }
        }

        public IEnumerable<ComponentNode> Components
        {
            get
            {
                return Ctx.Data.Graph.NodeItems.OfType<ISystemGroupProvider>().SelectMany(p => p.GetSystemGroups()).OfType<ComponentNode>().Concat(Ctx.Data.Graph.NodeItems.OfType<ComponentNode>()).Distinct();
            }
        }

        public IEnumerable<IMappingsConnectable> Groups
        {
            get
            {
                return Ctx.Data.Graph.NodeItems.OfType<ISystemGroupProvider>().SelectMany(p => p.GetSystemGroups()).Concat(Ctx.Data.Graph.NodeItems.OfType<IMappingsConnectable>()).Distinct();
            }
        }

        public IEnumerable<HandlerNode> EventHandlers
        {
            get
            {
                return Ctx.Data.EventHandlers;
            }
        }


        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            //    Ctx.CurrentDeclaration.Name = Ctx.Data.Name;
            if (!Ctx.IsDesignerFile)
            {
                Ctx.CurrentDeclaration.BaseTypes.Clear();
            }
        }

        public TemplateContext<SystemNode> Ctx { get; set; }
    }
    [TemplateClass(TemplateLocation.DesignerFile), AsPartial]
    [RequiresNamespace("uFrame.Kernel")]
    [WithMetaInfo]
    [AutoNamespaces]
    [NamespacesFromItems]
    public partial class SystemPartialTemplate : IClassTemplate<SystemNode>, ITemplateCustomFilename
    {
        public string Filename
        {
            get
            {
                if (Ctx.Data.Name == null)
                {
                   throw new Exception(Ctx.Data.Name + " Graph name is null");
                }
                if (Ctx.IsDesignerFile)
                {
                    return Path2.Combine("Systems", Ctx.Data.Name + ".designer.cs");
                }
                return Path2.Combine("Systems", Ctx.Data.Name + ".cs");
            }
        }
        public string OutputPath
        {
            get { return Path2.Combine("Systems", Ctx.Data.Name + ".cs"); }
        }

        public IEnumerable<ComponentNode> Components
        {
            get
            {
                return Ctx.Data.Graph.NodeItems.OfType<ISystemGroupProvider>().SelectMany(p => p.GetSystemGroups()).OfType<ComponentNode>().Concat(Ctx.Data.Graph.NodeItems.OfType<ComponentNode>()).Distinct();
            }
        }

        public IEnumerable<IMappingsConnectable> Groups
        {
            get
            {
                return Ctx.Data.Graph.NodeItems.OfType<ISystemGroupProvider>().SelectMany(p => p.GetSystemGroups()).Concat(Ctx.Data.Graph.NodeItems.OfType<IMappingsConnectable>()).Distinct();
            }
        }

        public IEnumerable<HandlerNode> EventHandlers
        {
            get
            {
                return Ctx.Data.EventHandlers;
            }
        }

        [GenerateProperty, WithField]
        public object Instance
        {
            get
            {

                Ctx.CurrentProperty.Type = new CodeTypeReference("static " + Ctx.Data.Name);

                return null;
            }
            set
            {

            }
        }

        [GenerateConstructor]
        public void _Name_()
        {
            Ctx._("Instance = this");
        }


        [ForEach("Variables"), GenerateProperty(TemplateLocation.DesignerFile), WithName, WithField(typeof(_ITEMTYPE_), typeof(SerializeField))]
        public _ITEMTYPE_ Variable { get; set; }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            Ctx.CurrentDeclaration.Name = Ctx.Data.Name;
            Ctx.CurrentDeclaration.BaseTypes.Clear();
            Ctx.CurrentDeclaration.BaseTypes.Add((Ctx.Data.Name + "Base").ToCodeReference());
            
        }

        public TemplateContext<SystemNode> Ctx { get; set; }
    }
    [TemplateClass(TemplateLocation.DesignerFile,AutoInherit = true)]
    [ AsPartial]
    [RequiresNamespace("uFrame.ECS")]
    [RequiresNamespace("UnityEngine")]
    [WithMetaInfo]
    [NamespacesFromItems]
    public partial class ComponentTemplate : IClassTemplate<ComponentNode>, ITemplateCustomFilename
    {
        public string Filename
        {
            get
            {
                return Path2.Combine("Components", Ctx.Data.Name + ".cs");
            }
        }
        // Not used now
        public string OutputPath
        {
            get { return Path2.Combine("Extensions", Ctx.Data.Graph.Name, "Components"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            this.Ctx.CurrentDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(
                typeof(ComponentId).ToCodeReference(),
                new CodeAttributeArgument(new CodePrimitiveExpression(Ctx.Data.ComponentId))
                ));
            Ctx.CurrentDeclaration.BaseTypes.Clear();
            if (Ctx.Data.BaseNode != null)
            {
                Ctx.SetBaseType(Ctx.Data.BaseNode.FullName);
            }
            else
            {
                Ctx.SetBaseType(typeof(EcsComponent));
            }
            foreach (
                var item in Descriptors)
            {
                Ctx.CurrentDeclaration.BaseTypes.Add(item.ContextTypeName);

            }

            if (Ctx.Data.BlackBoard)
                Ctx.CurrentDeclaration.BaseTypes.Add(typeof(IBlackBoardComponent));

            foreach (var item in Ctx.Data.GetMembers())
            {
                Ctx.TryAddNamespace(item.MemberType.Namespace);
            }
        }

        public IEnumerable<DescriptorNode> Descriptors
        {
            get { return Ctx.Data.PersistedItems.OfType<IDescriptorItem>().SelectMany(p => p.Descriptors).Distinct(); }
        }

        [ForEach("Descriptors"), GenerateProperty]
        public PropertyInfo[] _Name_Members
        {
            get
            {
                var field = Ctx.CurrentDeclaration._private_("static System.Reflection.PropertyInfo[]", "_" + Ctx.Item.Name + "Members");
                Ctx._if("{0} == null", field.Name)
                    .TrueStatements._("{0} = this.GetDescriptorProperties<{1}Attribute>().ToArray()", field.Name, Ctx.Item.Name);
                Ctx._("return {0}", field.Name);
                return null;
            }
        }
        [TemplateComplete]
        public void TemplateComplete()
        {
            foreach (var item in Ctx.Data.Properties.Where(p => p.HideInUnityInspector))
            {
                var field = this.Ctx.CurrentDeclaration.Members.OfType<CodeMemberField>()
                    .FirstOrDefault(p => p.Name == "_" + item.Name);
                if (field != null)
                {
                    field.CustomAttributes.Add(new CodeAttributeDeclaration(typeof (HideInInspector).ToCodeReference()));
                }
            }
        }
        public TemplateContext<ComponentNode> Ctx { get; set; }
    }
    [TemplateClass(TemplateLocation.EditableFile)]
    [AsPartial]
    [RequiresNamespace("uFrame.ECS")]
    [RequiresNamespace("UnityEngine")]
    [NamespacesFromItems]
    public partial class ComponentEditableTemplate : IClassTemplate<ComponentNode>, ITemplateCustomFilename , IOnDemandTemplate
    {
        public string Filename
        {
            get
            {
                return Path2.Combine("Custom", Ctx.Data.Name + ".cs");
            }
        }
        // Not used now
        public string OutputPath
        {
            get { return Path2.Combine("Extensions", Ctx.Data.Graph.Name, "Components"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            Ctx.CurrentDeclaration.BaseTypes.Clear();
            foreach (var item in Ctx.Data.GetMembers())
            {
                Ctx.TryAddNamespace(item.MemberType.Namespace);
            }
        }

        [TemplateComplete]
        public void TemplateComplete()
        {
        }
        public TemplateContext<ComponentNode> Ctx { get; set; }
    }


    [TemplateClass(TemplateLocation.DesignerFile, "{0}Group"), AsPartial]
    [RequiresNamespace("uFrame.ECS.Components")]
    [RequiresNamespace("uFrame.ECS.APIs")]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("UniRx")]
    public partial class GroupTemplate : IClassTemplate<GroupNode>,ITemplateCustomFilename
    {

        public IEnumerable<ComponentNode> SelectComponents
        {
            get
            {
                return Ctx.Data.Require.Select(p => p.SourceItem).OfType<ComponentNode>();
            }
        }
         public string Filename
        {
            get
            {
                return Path2.Combine("Groups", Ctx.Data.Name + "Group.cs");
            }
        }

        public string OutputPath { get; set; }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {

            this.Ctx.SetBaseType("ReactiveGroup<{0}>", Ctx.Data.Name);
        }

        public TemplateContext<GroupNode> Ctx { get; set; }
    }

    [TemplateClass(TemplateLocation.DesignerFile, "{0}"), AsPartial]
    [RequiresNamespace("uFrame.ECS")]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("UniRx")]
    public partial class GroupItemTemplate : GroupItem, IClassTemplate<GroupNode>, ITemplateCustomFilename
    {

        public IEnumerable<ComponentNode> SelectComponents
        {
            get
            {
                return Ctx.Data.Require.Select(p => p.SourceItem).OfType<ComponentNode>();
            }
        }
        public string OutputPath
        {
            get { return Path2.Combine("Modules", Ctx.Data.Graph.Name, "Groups"); }
        }
        public string Filename
        {
            get
            {
                return Path2.Combine("Groups", Ctx.Data.Name + ".cs");
            }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            this.Ctx.CurrentDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(
                typeof (ComponentId).ToCodeReference(),
                new CodeAttributeArgument(new CodePrimitiveExpression(Ctx.Data.ComponentId))
                ));
        }

        public TemplateContext<GroupNode> Ctx { get; set; }
    }

    [TemplateClass(TemplateLocation.DesignerFile), AsPartial]
    [RequiresNamespace("uFrame.ECS")]
    [AutoNamespaces]
    [NamespacesFromItems]
    public partial class EventTemplate : IClassTemplate<EventNode>, ITemplateCustomFilename
    {
        public IEnumerable<PropertiesChildItem> Properties
        {
            get
            {
                foreach (var item in Ctx.Data.Properties)
                {
                    if (item.Name == "EntityId" && Ctx.Data.Dispatcher) continue;
                    yield return item;
                }
            }
        }
        public string Filename
        {
            get
            {
                return Path2.Combine("Events", Ctx.Data.Name + ".cs");
            }
        }
        public string OutputPath
        {
            get { return Path2.Combine("Library", Ctx.Data.Graph.Name, "Events"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            this.Ctx.CurrentDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(
                typeof(EventId).ToCodeReference(),
                new CodeAttributeArgument(new CodePrimitiveExpression(Ctx.Data.EventId))
                ));
            //this.Ctx.CurrentDeclaration.CustomAttributes.Add(
            //    new CodeAttributeDeclaration(
            //        typeof(uFrameEvent).ToCodeReference(), new CodeAttributeArgument(new CodePrimitiveExpression(Ctx.Data.Name))
            //        ));
            //if (Ctx.Data.Dispatcher)
            //{
            //    this.Ctx.CurrentDeclaration.Name += "Dispatcher";
            //    this.Ctx.SetBaseType(typeof(EcsDispatcher));
            //}
            //else
            //{
            this.Ctx.CurrentDeclaration.Name = Ctx.Data.Name;
                if (!Ctx.IsDesignerFile)
                {
                    this.Ctx.CurrentDeclaration.BaseTypes.Clear();
                }
            //}
        }

        public TemplateContext<EventNode> Ctx { get; set; }
    }

    [TemplateClass(TemplateLocation.DesignerFile), ForceBaseType(typeof (UFAction)), AsPartial]
    [RequiresNamespace("uFrame.ECS")]
    [RequiresNamespace("UnityEngine")]
    [NamespacesFromItems]
    public partial class CustomActionSequenceTemplate : SequenceTemplate<CustomActionNode>, ITemplateCustomFilename
    {
        public override string Filename
        {
            get { return Path2.Combine("CustomActions", Ctx.Data.Name + ".cs"); }
        }

        public override bool CanGenerate
        {
            get { return !Ctx.Data.CodeAction; }
        }
        [TemplateSetup]
        public void Setup()
        {
            Ctx.CurrentDeclaration.CustomAttributes.Add(
                new CodeAttributeDeclaration(typeof (ActionTitle).ToCodeReference(),
                    new CodeAttributeArgument(
                        new CodePrimitiveExpression(string.IsNullOrEmpty(Ctx.Data.ActionTitle)
                            ? Ctx.Data.Name
                            : Ctx.Data.ActionTitle))));
            foreach (var item in Ctx.Data.Inputs)
            {
                var field = Ctx.CurrentDeclaration._public_(item.RelatedTypeName, item.Name);
                field.CustomAttributes.Add(new CodeAttributeDeclaration(typeof (In).ToCodeReference(),
                    new CodeAttributeArgument(new CodePrimitiveExpression(item.Name))));
            }
            foreach (var item in Ctx.Data.Outputs)
            {
                var field = Ctx.CurrentDeclaration._public_(item.RelatedTypeName, item.Name);
                field.CustomAttributes.Add(new CodeAttributeDeclaration(typeof (Out).ToCodeReference(),
                    new CodeAttributeArgument(new CodePrimitiveExpression(item.Name))));
            }
            foreach (var item in Ctx.Data.Branches)
            {
                var field = Ctx.CurrentDeclaration._public_(typeof (Action), item.Name);
                field.CustomAttributes.Add(new CodeAttributeDeclaration(typeof (Out).ToCodeReference(),
                    new CodeAttributeArgument(new CodePrimitiveExpression(item.Name))));
            }
        }

        [TemplateComplete]
        public void PostProcess()
        {
            if (DebugSystem.IsDebugMode)
            {
                var executeMethod =
                    Ctx.CurrentDeclaration.Members.OfType<CodeMemberMethod>().FirstOrDefault(p => p.Name == "Execute");

                // rename it to perform
                if (executeMethod != null)
                {
                    executeMethod.Name = "Perform";
                }
            }
        }
   
    }

    [TemplateClass(TemplateLocation.DesignerFile), ForceBaseType(typeof(UFAction)), AsPartial]
    [RequiresNamespace("uFrame.ECS")]
    [RequiresNamespace("UnityEngine")]
    [NamespacesFromItems]
    public partial class CustomActionDesignerTemplate : IClassTemplate<CustomActionNode>, ITemplateCustomFilename
    {
        public string Filename
        {
            get { return Path2.Combine("CustomActions", Ctx.Data.Name + ".designer.cs"); }
        }
        public string OutputPath
        {
            get { return Path2.Combine("CustomActions", Ctx.Data.Name + ".designer.cs"); }
        }

        public bool CanGenerate
        {
            get { return Ctx.Data.CodeAction; }
        }

        public void TemplateSetup()
        {
            Ctx.CurrentDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(ActionTitle).ToCodeReference(),
                        new CodeAttributeArgument(new CodePrimitiveExpression(string.IsNullOrEmpty(Ctx.Data.ActionTitle) ? Ctx.Data.Name : Ctx.Data.ActionTitle))));
            foreach (var item in Ctx.Data.Inputs)
            {
                var field = Ctx.CurrentDeclaration._public_(item.RelatedTypeName, item.Name);
                field.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(In).ToCodeReference(),
                    new CodeAttributeArgument(new CodePrimitiveExpression(item.Name))));
            }
            foreach (var item in Ctx.Data.Outputs)
            {
                var field = Ctx.CurrentDeclaration._public_(item.RelatedTypeName, item.Name);
                field.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(Out).ToCodeReference(),
                    new CodeAttributeArgument(new CodePrimitiveExpression(item.Name))));
            }
            foreach (var item in Ctx.Data.Branches)
            {
                var field = Ctx.CurrentDeclaration._public_(typeof(Action), item.Name);
                field.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(Out).ToCodeReference(),
                    new CodeAttributeArgument(new CodePrimitiveExpression(item.Name))));
            }
        }

        public TemplateContext<CustomActionNode> Ctx { get; set; }
        
    }

    [TemplateClass(TemplateLocation.EditableFile), ForceBaseType(typeof(UFAction)), AsPartial]
    [RequiresNamespace("uFrame.ECS")]
    [NamespacesFromItems]
    public partial class CustomActionEditableTemplate : IClassTemplate<CustomActionNode>, ITemplateCustomFilename
    {

        public string OutputPath
        {
            get { return Path2.Combine("CustomActions", Ctx.Data.Name + ".cs"); }
        }

        public bool CanGenerate
        {
            get { return Ctx.Data.CodeAction; }
        }

        public void TemplateSetup()
        {
            this.Ctx.CurrentDeclaration.BaseTypes.Clear();
            Ctx.CurrentDeclaration.public_override_func(typeof(void), "Execute");
            
        }

        public TemplateContext<CustomActionNode> Ctx { get; set; }

        public string Filename
        {
            get { return Path2.Combine("CustomActions", Ctx.Data.Name + ".cs"); }
        }
    }



    [TemplateClass(TemplateLocation.DesignerFile), ForceBaseType(typeof(UFAction)), AsPartial]
    [RequiresNamespace("uFrame.ECS")]
    [RequiresNamespace("UnityEngine")]
    [RequiresNamespace("uFrame.Attributes")]
    public partial class CodeActionDesignerTemplate : IClassTemplate<CodeActionNode>, ITemplateCustomFilename
    {
        public string Filename
        {
            get { return Path2.Combine("CodeActions", Ctx.Data.Name + ".designer.cs"); }
        }
        public string OutputPath
        {
            get { return Path2.Combine("CustomActions", Ctx.Data.Name + ".designer.cs"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            Ctx.CurrentDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(ActionTitle).ToCodeReference(),
                        new CodeAttributeArgument(new CodePrimitiveExpression(Ctx.Data.Name))));
    
        }

        public TemplateContext<CodeActionNode> Ctx { get; set; }

    }

    [TemplateClass(TemplateLocation.EditableFile), ForceBaseType(typeof(UFAction)), AsPartial]
    [RequiresNamespace("uFrame.ECS")]
    [RequiresNamespace("uFrame.Attributes")]
    [RequiresNamespace("UnityEngine")]
    public partial class CodeActionEditableTemplate : IClassTemplate<CodeActionNode>, ITemplateCustomFilename
    {

        public string OutputPath
        {
            get { return Path2.Combine("CodeActions", Ctx.Data.Name + ".cs"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            this.Ctx.CurrentDeclaration.BaseTypes.Clear();
            Ctx.CurrentDeclaration.public_override_func(typeof(void), "Execute");

        }

        public TemplateContext<CodeActionNode> Ctx { get; set; }

        public string Filename
        {
            get { return Path2.Combine("CustomActions", Ctx.Data.Name + ".cs"); }
        }
    }

    public class _CONTEXTITEM_ : _ITEMTYPE_
    {
        public override string TheType(TemplateContext context)
        {
            return base.TheType(context);
        }
    }

    public class _CONTEXT_ : _ITEMTYPE_
    {
        public override string TheType(TemplateContext context)
        {
            return base.TheType(context) + "Context";
        }
    }
}