using System;

namespace uFrame.MVVM.ViewModels
{
    public interface ISignal
    {
        Type SignalType { get; }
        void Publish(object data);
        void Publish();
    }

    public interface ISignal<in T> : ISignal
    {
        void Publish(T data);
    }
}