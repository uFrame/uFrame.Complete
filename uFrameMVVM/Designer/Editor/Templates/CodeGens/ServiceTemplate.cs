using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using uFrame.Kernel;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;
using uFrame.MVVM.ViewModels;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both)]
    [AutoNamespaces]
    [NamespacesFromItems]
    public partial class ServiceTemplate : IClassTemplate<ServiceNode>, ITemplateCustomFilename 
    {
        public TemplateContext<ServiceNode> Ctx { get; set; }

        public string Filename
        {
            get
            {
                if (Ctx.Data.Name == null)
                {
                    throw new Exception(Ctx.Data.Name + " Graph name is empty");
                }
                return Ctx.IsDesignerFile ? Path2.Combine(Ctx.Data.Graph.Name, "Services.designer.cs") 
                                          : Path2.Combine(Ctx.Data.Graph.Name + "/Services", Ctx.Data.Name + ".cs");
            }
        }

        // Replace by ITemplateCustomFilename's Filename
        public string OutputPath { get { return ""; } }

        public bool CanGenerate { get { return true; } }

        public void TemplateSetup()
        {
            
        }
    }

    [ForceBaseType(typeof(SystemServiceMonoBehavior))]
    [RequiresNamespace("UnityEngine")]
    [RequiresNamespace("UniRx")]
    [RequiresNamespace("uFrame.IOC")]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.MVVM")]
    public partial class ServiceTemplate
    {
        [GenerateMethod, AsOverride, Inside(TemplateLocation.Both)]
        public void Setup()
        {
            Ctx.CurrentMethod.invoke_base();

            if(Ctx.IsDesignerFile)
            {
                foreach(var command in Ctx.Data.Handlers.Select(p => p.SourceItemObject).OfType<IClassTypeNode>())
                {
                    Ctx._("this.OnEvent<{0}>().Subscribe(this.{1}Handler)", command.ClassName, command.Name);
                }
            }
            else
            {
                Ctx.CurrentMethod.Comments.Add(new CodeCommentStatement("<summary>", true));
                Ctx.CurrentMethod.Comments.Add(new CodeCommentStatement("This method is invoked whenever the kernel is loading", true));
                Ctx.CurrentMethod.Comments.Add(new CodeCommentStatement("Since the kernel lives throughout the entire lifecycle  of the game, this will only be invoked once.", true));
                Ctx.CurrentMethod.Comments.Add(new CodeCommentStatement("</summary>", true));
                Ctx._comment("Use the line below to subscribe to events");
                Ctx._comment("this.OnEvent<MyEvent>().Subscribe(myEventInstance => {{ TODO }});");
            }
        }

        public IEnumerable<IDiagramNodeItem> Handlers
        {
            get{ return Ctx.Data.Handlers.Select(p => p.SourceItemObject); }
        }

        [ForEach("Handlers"), GenerateMethod, Inside(TemplateLocation.Both)]
        public virtual void _Name_Handler(ViewModelCommand data)
        {
            Ctx.CurrentMethod.Name = Ctx.Item.Name + "Handler";
            Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.ItemAs<IClassTypeNode>().ClassName);
            if(Ctx.IsDesignerFile) return;
            Ctx._comment("Process the commands information. Also, you can publish new events by using the line below.");
            Ctx._comment("this.Publish(new AnotherEvent())");

            Ctx.CurrentMethod.Comments.Add(new CodeCommentStatement("<sumarry>", true));
            Ctx.CurrentMethod.Comments.Add(
                new CodeCommentStatement(string.Format("This method is executed when using this.Publish(new {0}())", 
                    Ctx.ItemAs<IClassTypeNode>().ClassName)));
            Ctx.CurrentMethod.Comments.Add(new CodeCommentStatement("</sumarry>", true));
        }
    }
}


