using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Trigger Enter 2D"), uFrameCategory("Unity Messages")]
    public class OnTriggerEnter2DDispatcher : EcsDispatcher
    {
        public Collider2D ColliderData { get; set; }
        public void OnTriggerEnter2D(Collider2D coll)
        {

            var colliderEntity = coll.gameObject.GetComponent<Entity>();
            if (colliderEntity == null) return;
            ColliderId = colliderEntity.EntityId;
            EntityId = Entity.EntityId;
            ColliderData = coll;
            Publish(this);
        }
        [uFrameEventMapping("Collider")]
        public int ColliderId { get; set; }

        public Collision2D CollisionData { get; set; }
    }
}