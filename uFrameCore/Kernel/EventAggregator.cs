using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using uFrame.Attributes;
using UniRx;

namespace uFrame.Kernel
{
    public interface IEventManager
    {
        int EventId { get; set; }
        Type For { get; }
    }

    public interface IEventManager<in TEvent> : IEventManager
    {
        void Publish(TEvent evt);
    }

    public class EventManager<TEvent> : IEventManager<TEvent>
    {
        private Subject<TEvent> _eventType;

        public Subject<TEvent> EventSubject
        {
            get { return _eventType ?? (_eventType = new Subject<TEvent>()); }
            set { _eventType = value; }
        }

        private int _eventId;
        public int EventId
        {
            get
            {
                if (_eventId > 0)
                    return _eventId;

                var eventIdAttribute =
                    For.GetCustomAttributes(typeof(EventId), true).FirstOrDefault() as
                        EventId;
                if (eventIdAttribute != null)
                {
                    return _eventId = eventIdAttribute.Identifier;
                }
                return _eventId;
            }
            set { _eventId = value; }
        }

        public Type For { get { return typeof(TEvent); } }
        public void Publish(TEvent evt)
        {
            if (_eventType != null)
            {
                _eventType.OnNext(evt);
            }
        }
    }

    public class EcsEventAggregator : IEventAggregator
    {
        private Dictionary<Type, IEventManager> _managers;

        public Dictionary<Type, IEventManager> Managers
        {
            get { return _managers ?? (_managers = new Dictionary<Type, IEventManager>()); }
            set { _managers = value; }
        }
        private Dictionary<int, IEventManager> _managersById;

        public Dictionary<int, IEventManager> ManagersById
        {
            get { return _managersById ?? (_managersById = new Dictionary<int, IEventManager>()); }
            set { _managersById = value; }
        }

        public IEventManager GetEventManager(int eventId)
        {
            if (ManagersById.ContainsKey(eventId))
                return ManagersById[eventId];
            return null;
        }
        //public IEventManager GetEventManager(Type type)
        //{
        //    if (ManagersById.ContainsKey(eventId))
        //        return ManagersById[eventId];
        //    return null;
        //}

        public IObservable<TEvent> GetEvent<TEvent>()
        {
            IEventManager eventManager;
            Type eventType = typeof(TEvent);
            if (!Managers.TryGetValue(eventType, out eventManager))
            {
                eventManager = new EventManager<TEvent>();
                Managers.Add(eventType, eventManager);
                var eventId = eventManager.EventId;
                if (eventId > 0)
                {
                    ManagersById.Add(eventId, eventManager);
                }
                else
                {
                    // create warning here that eventid attribute is not set
                }
            }
            var em = eventManager as EventManager<TEvent>;
            if (em == null) return null;
            return em.EventSubject;
        }

        public void Publish<TEvent>(TEvent evt)
        {
            IEventManager eventManager;

            if (!Managers.TryGetValue(typeof(TEvent), out eventManager))
            {
                // No listeners anyways
                return;
            }
            IEventManager<TEvent> eventManagerTyped = (IEventManager<TEvent>) eventManager;
            eventManagerTyped.Publish(evt);
        }
    }
}
