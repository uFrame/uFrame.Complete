using System;
using System.Collections.Generic;
using uFrame.ECS.Components;
using UniRx;

namespace uFrame.ECS.APIs
{
    public interface IComponentSystem : IEcsSystem
    {
  		IObservable<IEcsComponent> ComponentCreatedObservable { get; } 
        IObservable<IEcsComponent> ComponentRemovedObservable { get; } 
        /// <summary>
        /// Gets a component manager for a specified component id.
        /// </summary>
        /// <param name="componentId">The id for the component type</param>
        /// <param name="manager">The manager that is found.</param>
        /// <returns>True if a manager is found.</returns>
        bool TryGetManager(int componentId, out IEcsComponentManager manager);

        /// <summary>
        /// Gets a component that existing on the entity with "entityId", and is of the the type "componentId".
        /// </summary>
        /// <param name="entityId">The id for the entity that has the component</param>
        /// <param name="componentId">The id for the component type</param>
        /// <param name="component">The components result.</param>
        /// <returns>True if a manager is found.</returns>
        bool TryGetComponent(int entityId, int componentId, out IEcsComponent component);
        bool TryGetComponent<TComponent>(int entityId, out TComponent component) where TComponent : class, IEcsComponent;
        bool TryGetComponent<TComponent>(int[] entityIds, out TComponent[] component) where TComponent : class, IEcsComponent;
        bool TryGetComponent<TComponent>(List<int> entityIds, out TComponent[] component) where TComponent : class, IEcsComponent;

        IEnumerable<TComponent> GetAllComponents<TComponent>() where TComponent : class, IEcsComponent;
        IEcsComponentManagerOf<TComponent> RegisterComponent<TComponent>(int componentId = 0) where TComponent : class, IEcsComponent;
        IEcsComponentManager RegisterComponent(Type componentType);
        void RegisterComponentInstance(Type componentType, IEcsComponent instance);
        void DestroyComponentInstance(Type componentType, IEcsComponent instance);
        void AddComponent(int entityId, Type componentType);
        void AddComponent<TComponentType>(int entityId) where TComponentType : class, IEcsComponent;

        TGroupType RegisterGroup<TGroupType, TComponent>(int componentId = 0) where TComponent : IEcsComponent
            where TGroupType : IReactiveGroup, new();


    }
}