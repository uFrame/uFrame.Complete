using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Collision Exit"), uFrameCategory("Unity Messages")]
    public class OnCollisionExitDispatcher : EcsDispatcher
    {
        [uFrameEventMapping("Collider")]
        public int ColliderId { get; set; }
        public void OnCollisionExit(Collision coll)
        {

            var colliderEntity = coll.collider.gameObject.GetComponent<Entity>();
            if (colliderEntity == null) return;
            ColliderId = colliderEntity.EntityId;
            EntityId = gameObject.GetComponent<Entity>().EntityId;
            CollisionData = coll;
            Publish(this);
        }

        public Collision CollisionData { get; set; }
    }
}