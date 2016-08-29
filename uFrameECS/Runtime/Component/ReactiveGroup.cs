using System;
using System.Collections.Generic;
using System.Linq;
using uFrame.ECS.APIs;
using uFrame.Kernel;
using UniRx;

namespace uFrame.ECS.Components
{
    /// <summary>
    /// Reactive Group is the base class of all group type components in ECS.
    /// </summary>
    /// <typeparam name="TContextItem"></typeparam>
    public class ReactiveGroup<TContextItem> : EcsComponentManagerOf<TContextItem>, IReactiveGroup where TContextItem : IEcsComponent
    {
        /// <summary>
        /// Does the given entity match this group filter, if so it will return the group item, otherwise, it will return null.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public TContextItem MatchAndSelect(int entityId)
        {
            if (_components.ContainsKey(entityId))
            {
                return _components[entityId];
            }
            return default(TContextItem);
        }

        /// <summary>
        /// This method is used to determine **when** to check that a entity still belongs to this group. It should also initially store any component managers needed for matching.
        /// Ex. If a HealthComponent belongs to a PlayerGroup then it should return ComponentSystem.RegisterComponent'HealthComponent'.CreatedObservable.Select(p=>p.EntityId)
        /// 
        /// This method is invoked from the EcsComponentService after all groups have been registered.
        /// </summary>
        /// <param name="ecsComponentService">The component system that is intalling this reactive-group.</param>
        /// <returns></returns>
        public virtual IEnumerable<IObservable<int>> Install(IComponentSystem ecsComponentService)
        {
            yield break;
        }

        /// <summary>
        /// Determine's wether or not an entity still belongs to this component or not and adjusts the list accordingly.
        /// </summary>
        /// <param name="entityId"></param>
        public void UpdateItem(int entityId)
        {
            if (Match(entityId))
            {
                var item = Select();
                AddItem(item);
            }
            else
            {
                if (_components.ContainsKey(entityId))
                    this.RemoveItem(_components[entityId]);
            }
        }

        /// <summary>
        /// Selects the last successfully matched item.
        /// </summary>
        /// <returns></returns>
        public virtual TContextItem Select()
        {
            return default(TContextItem);
        }
    }

    /// <summary>
    /// A Type group reactively keeps up with all components that have a specific base class, or interface implementation.
    /// This can be useful for queries such as. GetAllINetworkable, GetAllSerializable
    /// </summary>
    /// <typeparam name="TInterface">The type at which to keep track of.</typeparam>
    public class DescriptorGroup<TInterface> : EcsComponentManagerOf<TInterface>, IReactiveGroup where TInterface : IEcsComponent
    {
        public IEnumerable<IObservable<int>> Install(IComponentSystem ecsComponentService)
        {
            ecsComponentService.ComponentCreatedObservable.Where(p=>p is TInterface).Subscribe(OnNext);
            ecsComponentService.ComponentRemovedObservable.Where(p=>p is TInterface).Subscribe(_=>RemoveItem(_));
            yield break;
        }

        private void OnNext(IEcsComponent _)
        {
            AddItem(_);
        }

        public void UpdateItem(int entityId)
        {
            // No need for updating the item its simply based on a type
        }
    }

    /// <summary>
    /// A Type group reactively keeps up with all components that have a specific attribute defined on the component decleration.
    /// </summary>
    /// <typeparam name="TAttributeType">The type at which to keep track of.</typeparam>
    public class AttributeGroup<TAttributeType> : EcsComponentManagerOf<IEcsComponent>, IReactiveGroup where TAttributeType : Attribute
    {
        public IEnumerable<IObservable<int>> Install(IComponentSystem ecsComponentService)
        {
            ecsComponentService.ComponentCreatedObservable.Where(p => p.GetType().IsDefined(typeof(TAttributeType),true)).Subscribe(OnNext).DisposeWith(ecsComponentService);
            ecsComponentService.ComponentRemovedObservable.Where(p => p.GetType().IsDefined(typeof(TAttributeType), true)).Subscribe(_ => RemoveItem(_)).DisposeWith(ecsComponentService);
            yield break;
        }

        private void OnNext(IEcsComponent _)
        {
            AddItem(_);
        }

        public void UpdateItem(int entityId)
        {
            // No need for updating the item its simply based on a type
        }
    }
}