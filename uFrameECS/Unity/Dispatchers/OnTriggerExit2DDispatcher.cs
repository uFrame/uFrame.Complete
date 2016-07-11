using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Trigger Exit 2D"), uFrameCategory("Unity Messages")]
    public class OnTriggerExit2DDispatcher : EcsDispatcher
    {
        public Collider2D ColliderData { get; set; }
        public void OnTriggerExit2D(Collider2D coll)
        {

            var colliderEntity = coll.gameObject.GetComponent<Entity>();
            if (colliderEntity == null) return;
            ColliderId = colliderEntity.EntityId;
            EntityId = Entity.EntityId;
            ColliderData = coll;
            Publish(this);
        }

        public int ColliderId { get; set; }

        public Collision2D CollisionData { get; set; }
    }
}