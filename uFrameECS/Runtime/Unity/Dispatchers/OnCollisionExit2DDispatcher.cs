using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Collision Exit 2D"), uFrameCategory("Unity Messages")]
    public class OnCollisionExit2DDispatcher : EcsDispatcher
    {
        public int ColliderId { get; set; }
        public void OnCollisionExit2D(Collision2D coll)
        {

            var colliderEntity = coll.collider.gameObject.GetComponent<Entity>();
            if (colliderEntity == null) return;
            ColliderId = colliderEntity.EntityId;
            EntityId = gameObject.GetComponent<Entity>().EntityId;
            CollisionData = coll;
            Publish(this);
        }

        public Collision2D CollisionData { get; set; }
    }
}