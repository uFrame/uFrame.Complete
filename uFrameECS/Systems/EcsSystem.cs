using System;
using System.Linq;
using uFrame.Kernel;
using UniRx;
using uFrame.ECS.APIs;
using uFrame.ECS.Components;
using uFrame.IOC;

namespace uFrame.ECS.Systems
{
    /// <summary>
    /// This is the base class for all systems.  It derives from SystemServiceMonoBehaviour which is part of the uFrame Kernel
    /// </summary>
    public abstract partial class EcsSystem : SystemServiceMonoBehavior, IEcsSystem
    {
        /// <summary>
        /// Access to the component system.  Use this to get/register components or groups.
        /// </summary>
        [Inject]
        public IComponentSystem ComponentSystem { get; set; }

        [Inject]
        public IBlackBoardSystem BlackBoardSystem { get; set; }

        /// <summary>
        /// The Ecs Event Aggregator, comes with additional features specific to ECS.
        /// </summary>
        public EcsEventAggregator EventSystem
        {
            get
            {
                return EventAggregator as EcsEventAggregator;
                
            }
        }

        public void EnsureDispatcherOnComponents<TDispatcher>(params Type[] forTypes) where TDispatcher : EcsDispatcher
        {
            this.OnEvent<ComponentCreatedEvent>().Where(p => forTypes.Contains(p.Component.GetType()))
                .Subscribe(_ =>
                {
                    var component = _.Component as EcsComponent;
                    if (component == null) return;

                    var d = component.gameObject.GetComponent<TDispatcher>();
                    if (d != null) return;
                    var entityId = component.EntityId;

                    component.gameObject
                        .AddComponent<TDispatcher>()
                        .EntityId = entityId
                        ;
                })
                .DisposeWith(this);
        }   
      
    }
}