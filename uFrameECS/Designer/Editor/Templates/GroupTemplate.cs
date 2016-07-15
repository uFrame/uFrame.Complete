using System.Collections.Generic;
using UniRx;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.ECS.APIs;

namespace uFrame.ECS.Templates
{
    [RequiresNamespace("uFrame.ECS.Systems")]
    public partial class GroupTemplate
    {
        [ForEach("SelectComponents"), GenerateProperty, WithField]
        public IEcsComponentManagerOf<_ITEMTYPE_> _Name_Manager { get; set; }
            
        [GenerateMethod, AsOverride]
        public virtual IEnumerable<IObservable<int>> Install(IComponentSystem componentSystem)
        {
            foreach (var item in Ctx.Data.Observables)
            {
                var node = item.Node;
                
                Ctx._("componentSystem.PropertyChangedEvent<{0}, {1}>(_ => _.{2}Observable, (c, v) => {{ UpdateItem(c.EntityId); }})",
                    node.Name,
                    item.RelatedTypeName,
                    item.Name);
            }

            foreach (var item in SelectComponents)
            {
                Ctx._("{0} = componentSystem.RegisterComponent<{1}>()", item.Name + "Manager", item.Name);
                Ctx._("yield return {0}.CreatedObservable.Select(_=>_.EntityId);", item.Name + "Manager");
                Ctx._("yield return {0}.RemovedObservable.Select(_=>_.EntityId);", item.Name + "Manager");
            }
            
            yield break;
        }

        [GenerateMethod(CallBase = false), AsOverride]
        public bool Match(int entityId)
        {
            Ctx.CurrentDeclaration._private_(typeof (int), "lastEntityId");
            Ctx._("lastEntityId = entityId");
            foreach (var item in SelectComponents)
            {
                Ctx.CurrentDeclaration._private_(item.Name, item.Name);
                
                Ctx._if("({0} = {1}Manager[entityId]) == null", item.Name,item.Name)
                    .TrueStatements._("return false");
            }
            if (Ctx.Data.ExpressionNode != null)
            {
                var exp = Ctx._if(Ctx.Data.ExpressionNode.GetExpression());
                exp.TrueStatements._("return true");
                exp.FalseStatements._("return false");
            }
            
            //return base.Match(entityId);
            Ctx._("return true");
            return true;
        }

        [GenerateMethod, AsOverride]
        public _CONTEXTITEM_ Select()
        {
            Ctx._("var item = new {0}();", new _CONTEXTITEM_().TheType(Ctx));
            Ctx._("item.EntityId = lastEntityId");
            foreach (var item in SelectComponents)
            {
                Ctx._("item.{0} = {1}", item.Name, item.Name);
            }
            Ctx._("return item");
            return null;
        }
    }
}