using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using uFrame.Attributes;
using uFrame.Kernel;
using UniRx;
using UnityEngine;
using uFrame.ECS.APIs;
using uFrame.ECS.Systems;
using uFrame.Json;

namespace uFrame.ECS.Components
{
    /// <summary>
    /// The base class for all ECS components, these components are nothing more than just data.  
    /// For the sake of Unity Compatability, it listens for a few Unity messages to make sure the ecs component system is always updated.
    /// </summary>
    [RequireComponent(typeof(Entity))]
    public class EcsComponent : uFrameComponent, IEcsComponent, IDisposableContainer
    {
        //[SerializeField]
        private int _entityId;

        private Transform _cachedTransform;

        /// <summary>
        /// The id for the entity that this component belongs to.  This id belongs to the IEcsComponent interface and is used
        /// for matching under the hood.
        /// </summary>
        [uFrameEventMapping("Source")]
        public virtual int EntityId
        {
            get
            {
                if (_entityId == 0)
                {
                    if (Entity != null)
                    {
                        _entityId = Entity.EntityId;
                    }
                    else 
                    {
                        _entityId = GetComponent<Entity>().EntityId;
                    }
                }

                return _entityId;
            }
            set { 
                _entityId = value;
            }
        }

        public virtual int ComponentId
        {
            get { return 0; }
        }

        /// <summary>
        /// Is this component enabled
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set { this.enabled = value; }
        }

        public bool IsDirty { get; set; }

        public Entity _Entity;
        private Subject<Unit> _changed;

        public void Reset()
        {
            var entityComponent = GetComponent<Entity>();
            if (entityComponent == null)
                entityComponent = gameObject.AddComponent<Entity>();
            Entity = entityComponent;
        }
        private void OnApplicationQuit()
        {
            IsQuiting = true;
        }

        /// <summary>
        /// A bool variable to determine if the application is quiting, useful in some situations.
        /// </summary>
        public bool IsQuiting { get; set; }


        public override void KernelLoaded()
        {
            IsQuiting = false;
            base.KernelLoaded();
            if (EntityId != 0)
            {
                EcsComponentService.Instance.RegisterComponentInstance(GetType(),this);
                return;
            }
          
            EcsComponentService.Instance.RegisterComponentInstance(this.GetType(), this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EcsComponentService.Instance.DestroyComponentInstance(this.GetType(), this);
 
        }

        /// <summary>
        /// A lazy loaded cached reference to the transform
        /// </summary>
        public Transform CachedTransform
        {
            get { return _cachedTransform ?? (_cachedTransform = transform); }
            set { _cachedTransform = value; }
        }

        public IObservable<Unit> Changed
        {
            get { return _changed ?? (_changed = new Subject<Unit>()); }
        }

        /// <summary>
        /// The actual entity component that this component belongs to.
        /// </summary>
        public Entity Entity
        {
            get { return _Entity ?? (_Entity = GetComponent<Entity>()); }
            set { _Entity = value; }
        }


        public void SetProperty<TValue>(ref TValue valueField, TValue value, ref PropertyChangedEvent<TValue> cachedEvent, Subject<PropertyChangedEvent<TValue>> observable = null)
        {
            var previousValue = valueField;
            valueField = value;
            IsDirty = true;
            if (observable != null)
            {
                if (cachedEvent == null)
                    cachedEvent = new PropertyChangedEvent<TValue>();

                cachedEvent.PreviousValue = previousValue;
                cachedEvent.CurrentValue = valueField;
                observable.OnNext(cachedEvent);
            }
      
        }
        /// <summary>
        /// Creates a new gamobject entity with the component attached to it.
        /// </summary>
        /// <param name="componentType">The type of component to add to the game object/entity.</param>
        /// <returns>The component created.</returns>
        public static IEcsComponent CreateObject(Type componentType) 
        {
            var go = new GameObject(componentType.Name);
            go.AddComponent<Entity>();
            return go.AddComponent(componentType) as IEcsComponent;
        }

        /// <summary>
        /// Creates a new gamobject entity with the component attached to it.
        /// </summary>
        /// <returns>The component created.</returns>
        public static TComponentType CreateObject<TComponentType>() where TComponentType : Component
        {
            var go = new GameObject(typeof(TComponentType).Name);
            go.AddComponent<Entity>();
            return go.AddComponent<TComponentType>();
        }
    }

    public class PropertyChangedEvent<TValue>
    {
        public TValue PreviousValue;
        public TValue CurrentValue;
    }



    //public class PropertyDescriptor<T> : IPropertyDescriptor
    //{
    //    public string Name { get; set; }

    //    public PropertyDescriptor(string name, Func<IObservable<T>> getObservable, Action<T> setValue, Func<T> getValue)
    //    {
    //        Name = name;
    //        GetObservable = getObservable;
    //        SetValue = setValue;
    //        GetValue = getValue;
    //    }

    //    public Func<IObservable<T>> GetObservable { get; set; } 

    //    public Action<T> SetValue { get; set; }

    //    public Func<T> GetValue { get; set; }
        
    //}

    public class TestComponent : EcsComponent
    {

        public IObservable<PropertyChangedEvent<Vector3>> OffsetObservable
        {
            get
            {
                if (_OffsetObservable == null)
                {
                    _OffsetObservable = new Subject<PropertyChangedEvent<Vector3>>();
                }
                return _OffsetObservable;
            }
        }

        public Vector3 Offset
        {
            get
            {
                return _Offset;
            }
            set
            {
                SetProperty(ref _Offset,value,ref _OffsetEvent,_OffsetObservable);
            }
        }

        private Subject<PropertyChangedEvent<Vector3>> _OffsetObservable;

        [UnityEngine.SerializeField()]
        private Vector3 _Offset;

        private PropertyChangedEvent<Vector3> _OffsetEvent;


    }

    public static class EcsComponentExtensions
    {
       
        public static IEnumerable<PropertyInfo> GetDescriptorProperties<TAttribute>(this object component)
        {

            return
                component.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.IsDefined(typeof(TAttribute), true));
        }

        public static JSONClass SerializeComponent<TDescriptorAttribute>(this object component)
        {
            var node = new JSONClass();
            foreach (var property in component.GetDescriptorProperties<TDescriptorAttribute>())
            {

                if (property.CanRead && property.CanWrite)
                {
                    if (property.PropertyType == typeof(int))
                    {
                        node.Add(property.Name, new JSONData((int)property.GetValue(component, null)));
                        continue;
                    }
                    if (property.PropertyType == typeof(bool))
                    {
                        node.Add(property.Name, new JSONData((bool)property.GetValue(component, null)));
                        continue;
                    }
                    if (property.PropertyType == typeof(float))
                    {
                        node.Add(property.Name, new JSONData((float)property.GetValue(component, null)));
                        continue;
                    }
                    if (property.PropertyType == typeof(Vector3))
                    {
                        node.Add(property.Name, new JSONData((Vector3)property.GetValue(component, null)));
                        continue;
                    }
                    if (property.PropertyType == typeof(Vector2))
                    {
                        node.Add(property.Name, new JSONData((Vector2)property.GetValue(component, null)));
                        continue;
                    }
                    if (property.PropertyType == typeof(string))
                    {
                        node.Add(property.Name, new JSONData((string)property.GetValue(component, null)));
                        continue;
                    }
                }
            }
            return node;
        }

        public static void DeserializeComponent<TDescriptorAttribute>(this object component, JSONNode node)
        {
            foreach (var property in component.GetDescriptorProperties<TDescriptorAttribute>())
            {
                ApplyValue(component, node, property);
            }
        }

        public static void ApplyValue(object component, JSONNode node, PropertyInfo property)
        {
            if (property.CanRead && property.CanWrite)
            {
                var propertyData = node[property.Name];
                if (property.PropertyType == typeof (int))
                {
                    property.SetValue(component, propertyData.AsInt, null);
                    return;
                }
                if (propertyData == null) return;
                if (property.PropertyType == typeof (bool))
                {
                    property.SetValue(component, propertyData.AsBool, null);
                    return;
                }
                if (property.PropertyType == typeof (float))
                {
                    property.SetValue(component, propertyData.AsFloat, null);
                    return;
                }
                if (property.PropertyType == typeof (Vector3))
                {
                    property.SetValue(component, propertyData.AsVector3, null);
                    return;
                }
                if (property.PropertyType == typeof (Vector2))
                {
                    property.SetValue(component, propertyData.AsVector2, null);
                    return;
                }
                if (property.PropertyType == typeof (string))
                {
                    property.SetValue(component, propertyData.Value, null);
                    return;
                }
            }
        }
    }


    public class PropertyDescriptor
    {
        //public PropertyInfo PropertyInfo { get; set; }

        public Type PropertyType { get; set; }

        public Func<IObservable<object>> GetObservable { get; set; }
        
        public Func<object> GetValue { get; set; }
        public Action<object> SetValue { get; set; } 
    }

}