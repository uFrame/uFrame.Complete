using System.Collections.Generic;
using UniRx;

namespace uFrame.ECS.APIs
{
    public interface IEcsComponentManagerOf<TComponentType> : IEcsComponentManager
    {
        TComponentType this[int entityId] { get; }
        List<TComponentType> Components { get; }
        IObservable<TComponentType> CreatedObservable { get; }
        IObservable<TComponentType> RemovedObservable { get; }
    }
}