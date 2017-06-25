using System;
using System.Collections.Generic;
using uFrame.Kernel;
using UniRx;

namespace uFrame.MVVM.ViewModels
{

    public class P<T> : ISubject<T>, IObservableProperty, ISimpleNotifyPropertyChanged
    {
        public static readonly EqualityComparer<T> EqualityComparer = EqualityComparer<T>.Default;
        public event PropertyChangedSimpleEventHandler PropertyChanged;

        private T _objectValue;
        private T _lastValue;

        public IObservable<T> ChangedObservable
        {
            get { return this.Where(p => !EqualityComparer.Equals(Value, LastValue)); }
        }

        public object ObjectValue
        {
            get { return Value; }
            set { Value = (T) value; }
        }

        public string PropertyName { get; set; }

#if !DLL
        public ViewModel Owner { get; set; }
#endif

        public IDisposable SubscribeInternal(Action<object> propertyChanged)
        {
            return this.Subscribe((v) => propertyChanged(v));
        }

        public P()
        {
        }

        //public IDisposable SubscribeInternal(Action<object> obj)
        //{
        //    this.Subscribe(obj);
        //}

        public T LastValue
        {
            get { return _lastValue; }
            set { _lastValue = value; }
        }

        public IDisposable Subscribe(IObserver<object> observer)
        {
            PropertyChangedSimpleEventHandler evt = (sender, name) => observer.OnNext(ObjectValue);
            PropertyChanged += evt;
            var disposer = Disposable.Create(() => PropertyChanged -= evt);
            if (Owner != null)
            {
                Owner.AddBinding(disposer);
            }
            return disposer;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this,propertyName);
            if (Owner != null)
                Owner.OnPropertyChanged(this, PropertyName);
        }

        public IObservable<Unit> AsUnit
        {
            get { return this.Select(p => Unit.Default); }
        }

        public Func<T> Computer { get; set; }

        public IDisposable ToComputed(Func<T> action, params IObservableProperty[] properties)
        {
            Computer = action;
            var disposables = new List<IDisposable>();
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                disposables.Add(property.SubscribeInternal(_ => { OnNext(action()); }));
            }

            // https://github.com/ZimM-LostPolygon/uFrame/commit/b3cd8cd9c1891845a9d73b79a4c6109809c06664
            //OnNext(action());

            return Disposable.Create(() =>
            {
                for (int i = 0; i < disposables.Count; i++)
                {
                    var d = disposables[i];
                    d.Dispose();
                }
            });
        }

        ///// <summary>
        ///// Makes this property a computed variable.
        ///// </summary>
        ///// <param name="action"></param>
        ///// <param name="properties"></param>
        ///// <returns></returns>
        //public IDisposable ToComputed(Func<ViewModel, T> action, params IObservableProperty[] properties)
        //{
        //    var disposables = new List<IDisposable>();
        //    foreach (var property in properties)
        //    {
        //        disposables.Add(property.SubscribeInternal(_ =>
        //        {
        //            OnNext(action(Owner));
        //        }));
        //    }
        //    action(Owner);

        //    return Disposable.Create(() =>
        //    {
        //        foreach (var d in disposables)
        //            d.Dispose();
        //    });
        //}

        public P(T value)
        {
            _objectValue = value;
        }

#if !DLL
        public P(ViewModel owner, string propertyName)
        {
            Owner = owner;
            PropertyName = propertyName;

        }
#endif

        public Type ValueType
        {
            get { return typeof (T); }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {

            PropertyChangedSimpleEventHandler evt = delegate { observer.OnNext(Value); };
            PropertyChanged += evt;
            return Disposable.Create(() => PropertyChanged -= evt);
        }

        public T Value
        {
            get { return _objectValue; }
            set {
                _lastValue = _objectValue;
                _objectValue = value;
                OnPropertyChanged(PropertyName);
            }
        }

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(T value)
        {
            Value = value;
        }
    }
}