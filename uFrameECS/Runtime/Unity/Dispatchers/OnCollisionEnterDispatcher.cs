using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Collision Enter"), uFrameCategory("Unity Messages")]
    public class OnCollisionEnterDispatcher : EcsDispatcher
    {
        [uFrameEventMapping("Collider")]
        public int ColliderId { get; set; }

        public Collision CollisionData { get; set; }

        public void OnCollisionEnter(Collision coll)
        {

            var colliderEntity = coll.collider.gameObject.GetComponent<Entity>();
            if (colliderEntity == null) return;
            ColliderId = colliderEntity.EntityId;
            EntityId = Entity.EntityId;
            CollisionData = coll;
            Publish(this);
        }
    }
}