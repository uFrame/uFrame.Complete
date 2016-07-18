using System.Linq;
using uFrame.ECS.APIs;
using uFrame.ECS.Editor;
using uFrame.ECS.Systems;
using uFrame.Editor.Compiling.CodeGen;

namespace uFrame.ECS.Templates
{
    [ForceBaseType(typeof(EcsSystem))]
    [RequiresNamespace("uFrame.ECS.Components")]
    [RequiresNamespace("uFrame.ECS.APIs")]
    [RequiresNamespace("UniRx")]
    [RequiresNamespace("UnityEngine")]

    public partial class SystemTemplate
    {


        [GenerateMethod(TemplateLocation.Both), AsOverride, InsideAll]
        public void Setup()
        {
            //Ctx._("Instance = this");
            Ctx.CurrentMethod.invoke_base();
   
            foreach (var item in Groups)
            {
                //Ctx._("{0}Manager = ComponentSystem.RegisterGroup<{0}Group,{0}>({1})", item.Name, item.ComponentId);
                var component = item as ComponentNode;
                if (component != null)
                {
                    Ctx._("{0}Manager = ComponentSystem.RegisterComponent<{0}>({1})", item.Name, component.ComponentId);
                    //if (component.BlackBoard)
                    //{
                    //    Ctx._("BlackBoardSystem.EnsureBlackBoard<{0}>()", item.Name);
                    //}
                }
                else
                {
                    Ctx._("{0}Manager = ComponentSystem.RegisterGroup<{0}Group,{1}>()", item.Name, item.ContextTypeName);
                }
                
            }
       
            foreach (var item in Ctx.Data.FilterNodes.OfType<ISetupCodeWriter>().ToArray())
            {
                item.WriteSetupCode(new CSharpSequenceVisitor() {_=Ctx}, Ctx);
            }
            if (!Ctx.IsDesignerFile)
            {
                Ctx.CurrentDeclaration.Members.Remove(Ctx.CurrentMethod);
            }
            
        }
        
        //[ForEach("Components"), GenerateProperty, WithField]
        //public IEcsComponentManagerOf<_ITEMTYPE_> _Name_Manager { get; set; }

        [ForEach("Groups"), GenerateProperty, WithField]
        public IEcsComponentManagerOf<_ITEM_TYPE_AS_INTERFACE> _Name_Manager { get; set; }
    }

    public class _ITEM_TYPE_AS_INTERFACE: _ITEMTYPE_
    {
        public override string TheType(TemplateContext context)
        {
            var item = context.Item as IMappingsConnectable;
            if (item != null)
            return item.ContextTypeName;
            return base.TheType(context);
        }
    }

    public partial class CustomActionEditableTemplate
    {
    
    }

    public partial class CustomActionDesignerTemplate
    {
        
    }
    //public IObservable<_ITEMTYPE_> _Name_Observable
    //{
    //    get
    //    {
    //        // return _MaxNavigatorsObservable ?? (_MaxNavigatorsObservable = new Subject<int>());
    //    }
    //}
    //public virtual Int32 MaxNavigators
    //{
    //    get
    //    {
    //        return _MaxNavigators;
    //    }
    //    set
    //    {
    //        _MaxNavigators = value;
    //        if (_MaxNavigatorsObservable != null)
    //        {
    //            _MaxNavigatorsObservable.OnNext(value);
    //        }
    //    }
    //}

    //[RequiresNamespace("uFrame.ECS")]
    //public partial class OnEventTemplate
    //{


    //}

    //[RequiresNamespace("uFrame.ECS")]
    //public partial class PublishTemplate
    //{


    //}
}

