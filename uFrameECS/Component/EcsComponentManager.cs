using System;
using System.Collections.Generic;
using System.Linq;
using uFrame.Attributes;
using uFrame.ECS.APIs;
using UniRx;

namespace uFrame.ECS.Components
{
    public abstract class EcsComponentManager : IEcsComponentManager
    {
        private int _componentId;
        public int ComponentId
        {
            get
            {
                if (_componentId > 0)
                    return _componentId;

                var componentIdAttribute =
                    For.GetCustomAttributes(typeof(ComponentId), true).FirstOrDefault() as
                        ComponentId;
                if (componentIdAttribute != null)
                {
                    return _componentId = componentIdAttribute.Identifier;
                }
                return _componentId;
            }
            set { _componentId = value; }
        }

        public abstract Type For { get; }
        

        public abstract IEnumerable<IEcsComponent> All { get; }

        public void RegisterComponent(IEcsComponent item)
        {
            AddItem(item);
        }

        public void UnRegisterComponent(IEcsComponent item)
        {
            RemoveItem(item);
        }

        public abstract IEcsComponent ForEntity(int entityId);


        protected abstract void AddItem(IEcsComponent component);
        protected abstract void RemoveItem(IEcsComponent component);

        public abstract bool Match(int entityId);
    }
}