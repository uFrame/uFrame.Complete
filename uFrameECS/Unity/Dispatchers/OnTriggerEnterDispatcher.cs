using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Trigger Enter"), uFrameCategory("Unity Messages")]
    public class OnTriggerEnterDispatcher : EcsDispatcher
    {
        [uFrameEventMapping("Collider")]
        public int ColliderId { get; set; }
        public Collider ColliderData { get; set; }
        public void OnTriggerEnter(Collider coll)
        {

            var colliderEntity = coll.GetComponent<Entity>();
            if (colliderEntity == null) return;
            ColliderId = colliderEntity.EntityId;
            EntityId = Entity.EntityId;
            ColliderData = coll;
            Publish(this);
        }

     
    }
}