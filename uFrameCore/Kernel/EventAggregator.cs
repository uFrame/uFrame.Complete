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
        private int _eventId;

        public Subject<TEvent> EventSubject
        {
            get { return _eventType ?? (_eventType = new Subject<TEvent>()); }
            set { _eventType = value; }
        }

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
        private Dictionary<int, IEventManager> _managersById;

        public Dictionary<Type, IEventManager> Managers
        {
            get { return _managers ?? (_managers = new Dictionary<Type, IEventManager>()); }
            set { _managers = value; }
        }

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
            EventManager<TEvent> em = (EventManager<TEvent>) eventManager;
            return em.EventSubject;
        }

        public void Publish<TEvent>(TEvent evt) {
            if (DebugEnabled)
            {
                PublishInternal(new DebugEventWrapperEvent(evt));    
            }
            PublishInternal(evt);
        }

        private void PublishInternal<TEvent>(TEvent evt)
        {
            IEventManager eventManager;

            Type eventType = typeof(TEvent);
            if (!Managers.TryGetValue(eventType, out eventManager))
            {
                // No listeners anyways
                return;
            }
            IEventManager<TEvent> eventManagerTyped = (IEventManager<TEvent>) eventManager;
            eventManagerTyped.Publish(evt);
        }

        public bool DebugEnabled { get; set; }
    }

    public struct DebugEventWrapperEvent
    {
        public readonly object Event;

        public DebugEventWrapperEvent(object evt)
        {
            Event = evt;
        }
    }
}
