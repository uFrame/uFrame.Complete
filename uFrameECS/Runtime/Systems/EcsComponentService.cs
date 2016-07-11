using System;
using System.Collections.Generic;
using System.Linq;
using uFrame.Kernel;
using UniRx;
using UnityEngine;
using uFrame.ECS.APIs;
using uFrame.ECS.Components;

namespace uFrame.ECS.Systems
{
    /// <summary>
    /// The main component service used to register and manage all components and groups.
    /// </summary>
    /// <code>
    /// // If you are inside of a system you can access this via 
    /// this.ComponentSystem
    /// // If you are inside of a code handler, you can access it via 
    /// System.ComponentSystem
    /// //If you are anywhere else you can access via 
    /// EcsComponentService.Instance
    /// </code>

    public class EcsComponentService : EcsSystem, IComponentSystem
    {
        /// <summary>
        /// The singleton instance property, this can be accessed from anywhere.
        /// </summary>
        public static EcsComponentService Instance { get; set; }

        public EcsComponentService()
        {
            Instance = this;
        }

        public override void Setup()
        {
            base.Setup();
            Instance = this;
        
        }

        public override void Loaded()
        {
            base.Loaded();
            var array = ComponentManagers.Where(p => p.Value is IReactiveGroup).ToArray();
            foreach (var item in array)
            {
                var reactiveGroup = item.Value as IReactiveGroup;
                if (reactiveGroup != null)
                {
                    foreach (var observable in reactiveGroup.Install(this))
                    {
                        observable.Subscribe(reactiveGroup.UpdateItem)
                            .DisposeWith(this);
                    }
                }
            }
        }

        private Dictionary<Type, IEcsComponentManager> _componentManager;
        private Dictionary<int, IEcsComponentManager> _componentManagerById;

        public LinkedList<Component> Components { get; set; }

        public Dictionary<Type, IEcsComponentManager> ComponentManagers
        {
            get { return _componentManager ?? (_componentManager = new Dictionary<Type, IEcsComponentManager>()); }
            set { _componentManager = value; }
        }
        public Dictionary<int, IEcsComponentManager> ComponentManagersById
        {
            get { return _componentManagerById ?? (_componentManagerById = new Dictionary<int, IEcsComponentManager>()); }
            set { _componentManagerById = value; }
        }
        /// <summary>
        /// Registers a component type with the component system.
        /// </summary>
        /// <param name="componentType">The type of component to register</param>
        /// <returns></returns>
        /// <code>
        /// var componentManager = System.ComponentSystem.RegisterComponent(typeof(PlayerComponent));
        /// foreach (var item in componentManager) {
        ///     Debug.Log(item.name);
        /// }
        /// </code>
        public IEcsComponentManager RegisterComponent(Type componentType)
        {
            IEcsComponentManager existing;
            if (!ComponentManagers.TryGetValue(componentType, out existing))
            {
                throw new Exception(string.Format("Component {0} not registered correctly.", componentType.Name));

            }
            return existing;
        }

        /// <summary>
        /// This method should be used to add any entity to the ecs component system
        /// 
        /// > You can use this if you want to register components that aren't derived from EcsComponent which requires MonoBehaviour, but you won't be able to see it in the unity inspector.
        /// </summary>
        /// <param name="componentType">The type of component to register.</param>
        /// <param name="instance">The actual instance that is being registered</param>
        /// <code>
        /// System.ComponentSystem.RegisterComponent(typeof(CustomComponent), new CustomComponent());
        /// </code>
        public void RegisterComponentInstance(Type componentType, IEcsComponent instance)
        {
            IEcsComponentManager existing;
            if (!ComponentManagers.TryGetValue(componentType, out existing))
            {
                var type = typeof(EcsComponentManagerOf<>).MakeGenericType(componentType);
                existing = Activator.CreateInstance(type) as EcsComponentManager;
                ComponentManagers.Add(componentType, existing);
                if (instance.ComponentId > 0)
                ComponentManagersById.Add(instance.ComponentId,existing);
            }
            if (_componentCreatedSubject != null)
            {
                _componentCreatedSubject.OnNext(instance);
            }
            existing.RegisterComponent(instance);
           
        }


        public void DestroyComponentInstance(Type componentType, IEcsComponent instance)
        {
            IEcsComponentManager existing;
            if (!ComponentManagers.TryGetValue(componentType, out existing))
            {
                return;
            }
            if (_componentRemovedSubject != null)
            {
                _componentRemovedSubject.OnNext(instance);
            }
            existing.UnRegisterComponent(instance);
          
        }

        public void AddComponent(int entityId, Type componentType)
        {
            //var entityManager = RegisterComponent<Entity>();
            //var entity = entityManager.Components.FirstOrDefault(p => p.EntityId == entityId);
            throw new Exception("Not Implemented use gameObject.AddComponent");
             
        }

        public void AddComponent<TComponentType>(int entityId) where TComponentType : class,  IEcsComponent
        {

        }

        public bool TryGetManager(int componentId, out IEcsComponentManager manager)
        {
            return ComponentManagersById.TryGetValue(componentId, out manager);
        }

        public IEcsComponentManagerOf<TComponent> RegisterComponent<TComponent>(int componentId = 0) where TComponent : class, IEcsComponent
        {
            IEcsComponentManager existing;
            if (!ComponentManagers.TryGetValue(typeof(TComponent), out existing))
            {
                existing = new EcsComponentManagerOf<TComponent>();
                existing.ComponentId = componentId;
                ComponentManagers.Add(typeof(TComponent), existing);
                //if (componentId > 0)
                //    ComponentManagersById.Add(componentId, existing);
                //else
                //{
                //    // Throw warning here?
                //}
                return (IEcsComponentManagerOf<TComponent>)existing;
            }
            else
            {
                return (IEcsComponentManagerOf<TComponent>)existing;
            }

        }


        /// <summary>
        /// Registers a a reactive group with the list of managers.  If the group already exists it will return it, if not it will create a new one and return that.
        /// </summary>
        /// <param name="componentId"></param>
        /// <typeparam name="TGroupType">The group type class. Usually derives from ReactiveGroup </typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns>The instance of the group manager.</returns>
        public TGroupType RegisterGroup<TGroupType, TComponent>(int componentId = 0) 
            where TComponent : IEcsComponent
            where TGroupType : IReactiveGroup, new()
        {
            IEcsComponentManager existing;
            if (!ComponentManagers.TryGetValue(typeof(TComponent), out existing))
            {
                existing = new TGroupType();
                ComponentManagers.Add(typeof(TComponent), existing);
                if (existing.ComponentId > 0)
                {
                    try
                    {
                        ComponentManagersById.Add(existing.ComponentId, existing);
                    }
                    catch (ArgumentException ex)
                    {
                        Debug.LogErrorFormat("Cannot register component {0} with ID {1}. Component with such Id is already registered: {2}.",typeof(TGroupType).Name,existing.ComponentId,
                            ComponentManagersById[existing.ComponentId].For.Name);
                    }
                }
                return (TGroupType)existing;
            }
            else
            {
                return (TGroupType)existing;
            }
        }

        public bool TryGetComponent(int entityId, int componentId, out IEcsComponent component)
        {
            IEcsComponentManager manager;
            if (ComponentManagersById.TryGetValue(componentId, out manager))
            {
                component = manager.ForEntity(entityId);
                return true;
            }
            component = null;
            return false;
        }

        public bool TryGetComponent<TComponent>(int[] entityIds, out TComponent[] components) where TComponent : class, IEcsComponent
        {
            var list = new List<TComponent>();
            foreach (var entityid in entityIds)
            {
                TComponent component;
                if (!TryGetComponent(entityid, out component))
                {
                    components = null;
                    return false;
                }
                list.Add(component);
            }
            components = list.ToArray();
            return true;
        }

        public bool TryGetComponent<TComponent>(List<int> entityIds, out TComponent[] components) where TComponent : class, IEcsComponent
        {
            var list = new List<TComponent>();
            foreach (var entityid in entityIds)
            {
                TComponent component;
                if (!TryGetComponent(entityid, out component))
                {
                    components = null;
                    return false;
                }
                list.Add(component);
            }
            components = list.ToArray();
            return true;
        }

        public IEnumerable<T> GetAllComponents<T>() where T : class, IEcsComponent
        {
            IEcsComponentManager existing;
            if (ComponentManagers.TryGetValue(typeof(T), out existing))
            {
                var manager = (EcsComponentManagerOf<T>)existing;
                foreach (var item in manager.Components)
                    yield return (T)item;

            }

        }

        private Subject<IEcsComponent> _componentCreatedSubject; 
        public IObservable<IEcsComponent> ComponentCreatedObservable
        {
            get
            {
                return _componentCreatedSubject ?? (_componentCreatedSubject = new Subject<IEcsComponent>());
            }
        }
        private Subject<IEcsComponent> _componentRemovedSubject;
        public IObservable<IEcsComponent> ComponentRemovedObservable
        {
            get
            {
                return _componentRemovedSubject ?? (_componentRemovedSubject = new Subject<IEcsComponent>());
            }
        }


        public bool TryGetComponent<TComponent>(int entityId, out TComponent component) where TComponent : class, IEcsComponent
        {
            IEcsComponentManager existing;
            if (!ComponentManagers.TryGetValue(typeof(TComponent), out existing))
            {
                component = null;
                return false;
            }
            var manager = (IEcsComponentManagerOf<TComponent>)existing;
            var result = manager.Components.FirstOrDefault(p => p.EntityId == entityId);
            if (result != null)
            {
                component = result;
                return true;
            }
            component = null;
            return false;
        }
    }
}