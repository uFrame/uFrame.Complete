using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using uFrame.MVVM.ViewModels;
using uFrame.MVVM.Views;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using UniRx;
using UniRx.Triggers;

namespace uFrame.MVVM.Bindings
{
    /// <summary>
    /// Binding extension methods that make it easy to bind ViewModels to Views, Any method that starts with Bind{...} will properly be unbound when a view is destroyed, if not
    /// it is the developers repsonsibility to properly dispose any subscriptions using the returned IDisposable.
    /// </summary>
    public static class ViewBindings
    {
        /// <summary>
        /// Bind to a ViewModel collection.
        /// </summary>
        /// <typeparam name="TCollectionItemType">The type that the collection contains.</typeparam>
        /// <param name="t">This</param>
        /// <param name="collection"></param>
        /// <param name="onAdd"></param>
        /// <param name="onRemove"></param>
        /// <returns>The binding class that allows chaining extra options.</returns>
        public static ModelCollectionBinding<TCollectionItemType> BindCollection<TCollectionItemType>(
            this IBindable t, ModelCollection<TCollectionItemType> collection,
            Action<TCollectionItemType> onAdd,
            Action<TCollectionItemType> onRemove

            )
        {
            var binding = new ModelCollectionBinding<TCollectionItemType>()
            {
                Collection = collection,
                OnAdd = onAdd,
                OnRemove = onRemove,

            };
            t.AddBinding(binding);
            binding.Bind();
            return binding;
        }

        /// <summary>
        /// Adds a binding to a collision, when the collusion occurs the call back will be invoked.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="eventType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IDisposable BindCollision(this ViewBase t, CollisionEventType eventType, Action<Collision> action)
        {
            return t.AddBinding(OnCollisionObservable(t.gameObject, eventType).Subscribe(action));
        }

        /// <summary>
        /// Bind a Unity Collision event to a ViewModel command.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="evenyType"></param>
        /// <returns></returns>
        public static IObservable<Collision> OnCollisionObservable(this GameObject t, CollisionEventType evenyType)
        {
            if(evenyType == CollisionEventType.Enter)
            {
                return t.OnCollisionEnterAsObservable();
            }
            else if(evenyType == CollisionEventType.Exit)
            {
                return t.OnCollisionExitAsObservable();
            }
            else
            {
                return t.OnCollisionStayAsObservable();
            }
        }


        /// <summary>
        /// Bind a Unity Collision event to a ViewModel command.
        /// </summary>
        /// <param name="t">The view that owns the binding</param>
        /// <param name="eventType">The collision event to bind to.</param>
        /// <returns>The collision binding class that allows chaining extra options.</returns>
        public static IObservable<Collision2D> OnCollision2DObservable(this GameObject t, CollisionEventType eventType)
        {
            if (eventType == CollisionEventType.Enter)
            {
                return t.EnsureComponent<ObservableCollisionEnter2DBehaviour>().OnCollisionEnter2DAsObservable();
            }
            else if (eventType == CollisionEventType.Exit)
            {
                return t.EnsureComponent<ObservableCollisionExit2DBehaviour>().OnCollisionExit2DAsObservable();
            }
            else
            {
                return t.EnsureComponent<ObservableCollisionStay2DBehaviour>().OnCollisionStay2DAsObservable();
            }
        }

        /// <summary>
        /// Bind a Unity Collision event to a ViewModel command.
        /// </summary>
        /// <param name="t">The view that owns the binding</param>
        /// <param name="eventType">The collision event to bind to.</param>
        /// <returns>The collision binding class that allows chaining extra options.</returns>
        public static IObservable<Collider> OnTriggerObservable(this GameObject t, CollisionEventType eventType)
        {
            if (eventType == CollisionEventType.Enter)
            {
                return t.EnsureComponent<ObservableTriggerEnterBehaviour>().OnTriggerEnterAsObservable();
            }
            else if (eventType == CollisionEventType.Exit)
            {
                return t.EnsureComponent<ObservableTriggerExitBehaviour>().OnTriggerExitAsObservable();
            }
            else
            {
                return t.EnsureComponent<ObservableTriggerStayBehaviour>().OnTriggerStayAsObservable();
            }
        }

        /// <summary>
        /// Bind a Unity Collision event to a ViewModel command.
        /// </summary>
        /// <param name="t">The view that owns the binding</param>
        /// <param name="eventType">The collision event to bind to.</param>
        /// <returns>The collision binding class that allows chaining extra options.</returns>
        public static IObservable<Collider2D> OnTrigger2DObservable(this GameObject t, CollisionEventType eventType)
        {
            if (eventType == CollisionEventType.Enter)
            {
                return t.EnsureComponent<ObservableTriggerEnter2DBehaviour>().OnTriggerEnter2DAsObservable();
            }
            else if (eventType == CollisionEventType.Exit)
            {
                return t.EnsureComponent<ObservableTriggerExit2DBehaviour>().OnTriggerExit2DAsObservable();
            }
            else
            {
                return t.EnsureComponent<ObservableTriggerStay2DBehaviour>().OnTriggerStay2DAsObservable();
            }
        }

        ///// <summary>
        ///// Ensures that a component exists and returns it.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public static T EnsureComponent<T>(this ViewBase t) where T : Component
        //{
        //    return t.GetComponent<T>() ?? t.gameObject.AddComponent<T>();
        //}

        /// <summary>
        /// Ensures that a component exists and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T EnsureComponent<T>(this GameObject t) where T : MonoBehaviour
        {
            if (t.GetComponent<T>() != null) return t.GetComponent<T>();
            return t.AddComponent<T>();
        }

        /// <summary>
        /// Creates a binding on collisions that filter to views only.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="eventType"></param>
        /// <param name="collision"></param>
        /// <returns></returns>
        public static IDisposable BindViewCollision(this ViewBase t, CollisionEventType eventType,
            Action<ViewBase> collision)
        {
            return t.AddBinding(OnViewCollision(t.gameObject, eventType).Subscribe(collision));
        }

        public static IDisposable BindViewCollisionWith<T>(this ViewBase t, CollisionEventType eventType,
            Action<T> collision) where T : ViewBase
        {
            return t.AddBinding(OnViewCollisionWith<T>(t.gameObject, eventType).Subscribe(collision));
        }

        public static IObservable<ViewBase> OnViewCollision(this GameObject t, CollisionEventType eventType)
        {
            return OnCollisionObservable(t, eventType).Select(p => p.GetView()).Where(p => p != null);
        }

        public static IObservable<T> OnViewCollisionWith<T>(this GameObject t, CollisionEventType eventType)
            where T : ViewBase
        {
            return OnCollisionObservable(t, eventType).Where(p => p.GetView<T>() != null).Select(p => p.GetView<T>());
        }

        /// <summary>
        /// Bind a key to a ViewModel Command
        /// </summary>
        /// <param name="t">The view that owns the binding</param>
        /// <param name="commandSelector"></param>
        /// <param name="key"></param>
        /// <returns>The binding class that allows chaining extra options.</returns>
        public static IDisposable BindKey<TCommandType>(this ViewBase t, Signal<TCommandType> commandSelector,
            KeyCode key, TCommandType parameter = null) where TCommandType : ViewModelCommand, new()
        {
            return
                t.AddBinding(
                    t.UpdateAsObservable()
                        .Where(p => Input.GetKey(key))
                        .Subscribe(
                            _ => commandSelector.OnNext(parameter ?? new TCommandType() {Sender = t.ViewModelObject})));
        }

        /// <summary>
        /// Bind a key to a ViewModel Command
        /// </summary>
        /// <param name="t">The view that owns the binding</param>
        /// <returns>The binding class that allows chaining extra options.</returns>
        public static IObservable<T> ScreenToRaycastAsObservable<T>(this ViewBase t, Func<RaycastHit, T> onHit)
        {
            return t.UpdateAsObservable().Select(p =>
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    return onHit(hit);
                }
                return default(T);
            });

        }

        /// <summary>
        /// Binds a mouse event to a ViewModel Command.
        /// </summary>
        /// <param name="view">The view that will own the Binding.</param>
        /// <param name="eventType">The mouse event to bind to.</param>
        /// <returns>The binding class that allows chaining extra options.</returns>
        public static IObservable<Unit> OnMouseEvent(this ViewBase view, MouseEventType eventType)
        {
            if (eventType == MouseEventType.OnMouseDown)
            {
                var component = view.AddComponentBinding<ObservableMouseDownBehaviour>();
                return component.OnMouseDownAsObservable();
            }
            else if (eventType == MouseEventType.OnMouseDrag)
            {
                var component = view.AddComponentBinding<ObservableMouseDragBehaviour>();
                return component.OnMouseDragAsObservable();
            }
            else if (eventType == MouseEventType.OnMouseEnter)
            {
                var component = view.AddComponentBinding<ObservableMouseEnterBehaviour>();
                return component.OnMouseEnterAsObservable();
            }
            else if (eventType == MouseEventType.OnMouseExit)
            {
                var component = view.AddComponentBinding<ObservableMouseExitBehaviour>();
                return component.OnMouseExitAsObservable();

            }
            else if (eventType == MouseEventType.OnMouseOver)
            {
                var component = view.AddComponentBinding<ObservableMouseOverBehaviour>();
                return component.OnMouseOverAsObservable();
            }
            return view.AddComponentBinding<ObservableMouseOverBehaviour>().OnMouseOverAsObservable();
        }

        public static IObservable<Unit> OnDestroyObservable(this GameObject gameObject)
        {
            return gameObject.EnsureComponent<ObservableOnDestroyBehaviour>().OnDestroyAsObservable();
        }

        public static IDisposable DisposeWith(this IDisposable disposable, GameObject gameObject)
        {
            return gameObject.OnDestroyObservable().First().Subscribe(p => disposable.Dispose());
        }

        //public static IDisposable DisposeWith(this IDisposable disposable, IBindable bindable)
        //{
        //    return bindable.AddBinding(disposable);
        //}

        public static IDisposable DisposeWhenChanged<T>(this IDisposable disposable, P<T> sourceProperty,
            bool onlyWhenChanged = true)
        {
            if (onlyWhenChanged)
            {
                var d =
                    sourceProperty.Where(p => !P<T>.EqualityComparer.Equals(sourceProperty.Value, sourceProperty.LastValue))
                        .First()
                        .Subscribe(_ => { disposable.Dispose(); });
                return d;
            }
            return sourceProperty.First().Subscribe(_ => { disposable.Dispose(); });

        }

        /// <summary>
        /// Binds a property to a view, this is the standard property binding extension method.
        /// </summary>
        /// <typeparam name="TBindingType"></typeparam>
        /// <param name="property"></param>
        /// <param name="bindable"></param>
        /// <param name="changed"></param>
        /// <param name="onlyWhenChanged"></param>
        /// <returns></returns>
        public static IDisposable BindProperty<TBindingType>(this IBindable bindable, P<TBindingType> property,
            Action<TBindingType> changed, bool onlyWhenChanged = true)
        {
            changed(property.Value);
            if (onlyWhenChanged)
            {
                return
                    bindable.AddBinding(
                        property.Where(p => !P<TBindingType>.EqualityComparer.Equals(property.Value, property.LastValue)).Subscribe(changed));
            }

            return bindable.AddBinding(property.Subscribe(changed));
        }

        /// <summary>
        /// Binds a property to a view, this is the standard property binding extension method.
        /// </summary>
        /// <typeparam name="TBindingType"></typeparam>
        /// <param name="property"></param>
        /// <param name="bindable"></param>
        /// <param name="changed"></param>
        /// <param name="onlyWhenChanged"></param>
        /// <returns></returns>
        public static IDisposable BindTwoWay<TBindingType>(this IBindable bindable, P<TBindingType> property,
            Action<TBindingType> changed, bool onlyWhenChanged = true)
        {
            changed(property.Value);
            if (onlyWhenChanged)
            {
                return
                    bindable.AddBinding(
                        property.Where(p => !P<TBindingType>.EqualityComparer.Equals(property.Value, property.LastValue)).Subscribe(changed));
            }

            return bindable.AddBinding(property.Subscribe(changed));
        }

        /// <summary>
        /// A wrapper of BindProperty for bindings in the diagram
        /// </summary>
        /// <typeparam name="TBindingType"></typeparam>
        /// <param name="bindable"></param>
        /// <param name="property"></param>
        /// <param name="changed"></param>
        /// <param name="onlyWhenChanged"></param>
        /// <returns></returns>
        public static IDisposable BindStateProperty<TBindingType>(this IBindable bindable, P<TBindingType> property,
            Action<TBindingType> changed, bool onlyWhenChanged = true)
        {
            return BindProperty(bindable, property, changed, onlyWhenChanged);
        }

        public static IDisposable BindEnum<TBindingType>(this IBindable bindable, P<TBindingType> property,
            Action<TBindingType> enumChanged, Action<TBindingType> enumChanged2)
        {

            return null;
        }

        /// <summary>
        /// Binds to a commands execution and is diposed with the bindable
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="sourceCommand"></param>
        /// <param name="executed"></param>
        /// <returns></returns>
        public static IDisposable BindCommandExecuted<TCommandType>(this ViewBase bindable,
            Signal<TCommandType> sourceCommand, Action<TCommandType> executed)
            where TCommandType : IViewModelCommand, new()
        {

            return bindable.AddBinding(sourceCommand.Subscribe(executed));
        }

        public static ModelViewModelCollectionBinding BindToViewCollection<TCollectionType>(this ViewBase view,
            ModelCollection<TCollectionType> viewModelCollection, Func<ViewModel, ViewBase> createView,
            Action<ViewBase> added,
            Action<ViewBase> removed,
            Transform parent,
            bool viewFirst = false)
        {
            var binding = new ModelViewModelCollectionBinding()
            {
                SourceView = view,
                ModelPropertySelector = () => viewModelCollection
            };
            binding.SetParent(parent);
            binding.SetAddHandler(added);
            binding.SetRemoveHandler(removed);
            binding.SetCreateHandler(createView);
            if (viewFirst)
            {
                binding.ViewFirst();
            }
            view.AddBinding(binding);
            binding.Bind();

            return binding;
        }
    }
}