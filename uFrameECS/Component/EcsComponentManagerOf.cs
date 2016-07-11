using System;
using System.Collections.Generic;
using System.Linq;
using uFrame.Attributes;
using uFrame.ECS.APIs;
using uFrame.Kernel;
using UniRx;
using UnityEngine;

namespace uFrame.ECS.Components
{
    public class EcsComponentManagerOf<TComponentType> : EcsComponentManager, IEcsComponentManagerOf<TComponentType> where TComponentType : IEcsComponent
    {
        private readonly List<TComponentType> _componentList = new List<TComponentType>();
    
        protected Dictionary<int, TComponentType> _components = new Dictionary<int, TComponentType>();

        private Subject<TComponentType> _CreatedObservable;


        public IObservable<TComponentType> CreatedObservable
        {
            get
            {
                if (_CreatedObservable == null)
                {
                    _CreatedObservable = new Subject<TComponentType>();
                }
                return _CreatedObservable;
            }
        }
        private Subject<TComponentType> _RemovedObservable;

        public IObservable<TComponentType> RemovedObservable
        {
            get
            {
                if (_RemovedObservable == null)
                {
                    _RemovedObservable = new Subject<TComponentType>();
                }
                return _RemovedObservable;
            }
        }

        public virtual List<TComponentType> Components
        {
            get
            {
                return _componentList;
            }
        }

        public override Type For
        {
            get { return typeof (TComponentType); }
        }

        public override IEnumerable<IEcsComponent> All
        {
            get
            {
                foreach (TComponentType component in Components)
                    yield return component;
            }
        }

        public TComponentType this[int entityId]
        {
            get
            {
                if (_components.ContainsKey(entityId))
                {
                    return _components[entityId];
                }
                return default(TComponentType);
            }
        }

        public override IEcsComponent ForEntity(int entityId)
        {
            if (!_components.ContainsKey(entityId)) return null;
            return _components[entityId];
        }

        protected override void AddItem(IEcsComponent component)
        {
            if (_components.ContainsKey(component.EntityId))
            {
                return;
            }
            else
            {
                _components.Add(component.EntityId, (TComponentType)component);
                _componentList.Add((TComponentType)component);
            }
            if (_CreatedObservable != null)
            {
                _CreatedObservable.OnNext((TComponentType) component);
            }
        }

        protected override void RemoveItem(IEcsComponent component)
        {
            if (component == null) return;
            if (_components == null || !_components.ContainsKey(component.EntityId)) return;
            
            _components.Remove(component.EntityId);
            _componentList.Remove((TComponentType) component);

            if (_RemovedObservable != null)
            {
                _RemovedObservable.OnNext((TComponentType)component);
            }
        }

        public override bool Match(int entityId)
        {
            return _components.ContainsKey(entityId);
        }
    }
}

