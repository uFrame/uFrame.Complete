using System.Collections.Generic;
using uFrame.ECS.APIs;
using UniRx;

namespace uFrame.ECS.Components
{
    public interface IReactiveGroup : IEcsComponentManager
    {
        /// <summary>
        /// This method is used to determine when to check that a group item is still valid. It should also initially store any component managers needed for matching.
        /// Ex. If a HealthComponent belongs to a PlayerGroup then it should return ComponentSystem.RegisterComponent'HealthComponent'.CreatedObservable.Select(p=>p.EntityId)
        /// </summary>
        /// <param name="ecsComponentService"></param>
        /// <returns></returns>
        IEnumerable<IObservable<int>> Install(IComponentSystem ecsComponentService);

        void UpdateItem(int entityId);
    }
}