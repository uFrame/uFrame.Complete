using System;
using System.Collections.Generic;
using System.Linq;
using uFrame.ECS.APIs;
using uFrame.ECS.Components;
using uFrame.Kernel;
using UniRx;

namespace uFrame.ECS.Systems
{
    /// <summary>
    /// These extensions are for facilitating the construction of systems. Component Created/Destroyed, Property Changes, Collection Modifications..etc
    /// </summary>
    public static class EcsSystemExtensions
    {

        

        //public static IEnumerable<IEcsComponent> MergeByEntity(this EcsSystem system, params IEcsComponentManager[] managers)
        //{
        //    var list = new HashSet<int>();
        //    foreach (var manager in managers)
        //    {
        //        foreach (var item in manager.All)
        //        {
        //            if (list.Contains(item.EntityId)) continue;
        //            yield return item;
        //            list.Add(item.EntityId);
        //        }
        //    }
        //}
        //public static void FilterWithDispatcher<TDispatcher>(this EcsSystem system, Func<TDispatcher, int> getMatchId, Action<int> handler, params Type[] forTypes)
        //    where TDispatcher : EcsDispatcher
        //{
        //    system.OnEvent<ComponentCreatedEvent>().Where(p => forTypes.Contains(p.Component.GetType()))
        //        .Subscribe(_ =>
        //        {
        //            var component = _.Component as EcsComponent;
        //            if (component == null) return;

        //            var d = component.gameObject.GetComponent<TDispatcher>();
        //            if (d != null) return;
        //            var entityId = component.EntityId;
                    
        //            component.gameObject
        //                .AddComponent<TDispatcher>()
        //                .EntityId = entityId
        //                ;

        //            system.OnEvent<TDispatcher>()
        //                .Where(p =>getMatchId(p) == component.EntityId)
        //                .Subscribe(x => handler(x.EntityId))
        //                .DisposeWith(system);
        //        })
        //        .DisposeWith(system);
        //    ;
        //}

        /// <summary>
        /// Listens for when a component is created, this also works for groups sinces components and group items both derive from IEcsComponent
        /// </summary>
        /// <typeparam name="TComponentType"></typeparam>
        /// <param name="system"></param>
        /// <returns></returns>
        public static IObservable<TComponentType> OnComponentCreated<TComponentType>(this IEcsSystem system) where TComponentType : class, IEcsComponent
        {
            return system.ComponentSystem.RegisterComponent<TComponentType>().CreatedObservable;
        }

        /// <summary>
        /// Listens for when a component is destroyed, this also works for groups sinces components and group items both derive from IEcsComponent
        /// </summary>
        /// <typeparam name="TComponentType"></typeparam>
        /// <param name="system"></param>
        /// <returns></returns>
        public static IObservable<TComponentType> OnComponentDestroyed<TComponentType>(this IEcsSystem system) where TComponentType : class, IEcsComponent
        {
            return system.ComponentSystem.RegisterComponent<TComponentType>().RemovedObservable;
        }


        public static IObservable<TComponentType> OnComponentCreated<TComponentType>(this IComponentSystem system) where TComponentType : class, IEcsComponent
        {
            return system.RegisterComponent<TComponentType>().CreatedObservable;
        }


        public static IObservable<TComponentType> OnComponentDestroyed<TComponentType>(this IComponentSystem system) where TComponentType : class, IEcsComponent
        {
            return system.RegisterComponent<TComponentType>().RemovedObservable;
        }


        /// <summary>
        /// Listens for when a property is changed and ensures that the subscription is properly disposed when the system disposes or when the component disposes.
        /// </summary>
        /// <typeparam name="TComponentType"></typeparam>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="system">This system to install this listner on, it will get disposed with this one.</param>
        /// <param name="select">A selector for the property observable on TComponentType that you wish to listen for.</param>
        /// <param name="handler">The method that is invoked when the property changes.</param>
        /// <param name="getImmediateValue">The lambda method for retreiving the primtive value, if this is not null, it will immedietly invoke the handler with this value.</param>
        /// <param name="onlyWhenChanged">Only invoke the method when the value actually changes rather than when it is set.</param>
        public static void PropertyChanged<TComponentType, TPropertyType>(this IEcsSystem system, 
            Func<TComponentType, 
            IObservable<TPropertyType>> select, 
            Action<TComponentType, 
            TPropertyType> handler, Func<TComponentType,TPropertyType> getImmediateValue = null, bool onlyWhenChanged = false ) where TComponentType : class, IEcsComponent
        {
            if (onlyWhenChanged)
            {
                system.OnComponentCreated<TComponentType>().DistinctUntilChanged().Subscribe(_ =>
                {
                    select(_).Subscribe(v => handler(_, v)).DisposeWith(_).DisposeWith(system);
                    if (getImmediateValue != null)
                    {
                        handler(_, getImmediateValue(_));

                    }

                }).DisposeWith(system);
            }
            else
            {
                system.OnComponentCreated<TComponentType>().Subscribe(_ =>
                {
                    select(_).Subscribe(v => handler(_, v)).DisposeWith(_).DisposeWith(system);
                    if (getImmediateValue != null)
                    {
                        handler(_, getImmediateValue(_));

                    }

                }).DisposeWith(system);
            }
          
        }


        /// <summary>
        /// Listens for when a property is changed and ensures that the subscription is properly disposed when the system disposes or when the component disposes.
        /// </summary>
        /// <typeparam name="TComponentType"></typeparam>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="system">This system to install this listner on, it will get disposed with this one.</param>
        /// <param name="select">A selector for the property observable on TComponentType that you wish to listen for.</param>
        /// <param name="handler">The method that is invoked when the property changes.</param>
        /// <param name="getImmediateValue">The lambda method for retreiving the primtive value, if this is not null, it will immedietly invoke the handler with this value.</param>
        /// <param name="onlyWhenChanged">Only invoke the method when the value actually changes rather than when it is set.</param>
        public static void PropertyChangedEvent<TComponentType, TPropertyType>(this IEcsSystem system,
            Func<TComponentType,
            IObservable<PropertyChangedEvent<TPropertyType>>> select,
            Action<TComponentType,
            PropertyChangedEvent<TPropertyType>> handler, Func<TComponentType, TPropertyType> getImmediateValue = null, bool onlyWhenChanged = false) where TComponentType : class, IEcsComponent
        {
            if (onlyWhenChanged)
            {
                system.OnComponentCreated<TComponentType>().Subscribe(_ =>
                {
                    select(_).Where(p=>!Equals(p.PreviousValue, p.CurrentValue)).Subscribe(v => handler(_, v)).DisposeWith(_).DisposeWith(system);
                    if (getImmediateValue != null)
                    {
                        handler(_, new PropertyChangedEvent<TPropertyType>()
                        {
                            CurrentValue = getImmediateValue(_)
                        });
                    }
                }).DisposeWith(system);
            }
            else
            {
                system.OnComponentCreated<TComponentType>().Subscribe(_ =>
                {
                    select(_).Subscribe(v => handler(_, v)).DisposeWith(_).DisposeWith(system);
                    if (getImmediateValue != null)
                    {
                        handler(_, new PropertyChangedEvent<TPropertyType>()
                        {
                            CurrentValue = getImmediateValue(_)
                        });

                    }

                }).DisposeWith(system);
            }

        }

        /// <summary>
        /// Listens for when a Reactive Collection's item has been added, and ensures that the subscription properly disposed when the system disposes or when the component disposes.
        /// </summary>
        /// <typeparam name="TComponentType"></typeparam>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="system">This system to install this listner on, it will get disposed with this one.</param>
        /// <param name="select">The ReactiveCollection selector.</param>
        /// <param name="handler">The method that is invoked when an item is added to the collection</param>
        /// <param name="immediate">Should the handler be invoked for every item that is currently in the list?</param>
        public static void CollectionItemAdded<TComponentType, TPropertyType>(this IEcsSystem system,
            Func<TComponentType,
            ReactiveCollection<TPropertyType>> select,
            Action<TComponentType,
            TPropertyType> handler, bool immediate = false) where TComponentType : class, IEcsComponent
        {

                system.OnComponentCreated<TComponentType>().Subscribe(_ =>
                {
                    select(_).ObserveAdd().Subscribe(v => handler(_, v.Value)).DisposeWith(_).DisposeWith(system);

                    if (immediate)
                    {
                        foreach (var item in select(_))
                        {
                            handler(_, item);
                        }
                    }

                }).DisposeWith(system);
        }

        /// <summary>
        /// Listens for when a Reactive Collection's item has been removed, and ensures that the subscription properly disposed when the system disposes or when the component disposes.
        /// </summary>
        /// <typeparam name="TComponentType"></typeparam>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="system">This system to install this listner on, it will get disposed with this one.</param>
        /// <param name="select">The ReactiveCollection selector.</param>
        /// <param name="handler">The method that is invoked when an item is added to the collection</param>        
        public static void CollectionItemRemoved<TComponentType, TPropertyType>(this IEcsSystem system,
          Func<TComponentType,
          ReactiveCollection<TPropertyType>> select,
          Action<TComponentType,
          TPropertyType> handler) where TComponentType : class, IEcsComponent
        {
            system.OnComponentCreated<TComponentType>().Subscribe(_ =>
            {
                select(_).ObserveRemove().Subscribe(v => handler(_, v.Value)).DisposeWith(_).DisposeWith(system);
            }).DisposeWith(system);
        }
    }
}