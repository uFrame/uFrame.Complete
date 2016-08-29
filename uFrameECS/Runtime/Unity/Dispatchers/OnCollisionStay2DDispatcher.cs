using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Collision Stay 2D"), uFrameCategory("Unity Messages")]
    public class OnCollisionStay2DDispatcher : EcsDispatcher
    {
        public int ColliderId { get; set; }
        public void OnCollisionStay2D(Collision2D coll)
        {

            var colliderEntity = coll.collider.gameObject.GetComponent<Entity>();
            if (colliderEntity == null) return;
            ColliderId = colliderEntity.EntityId;
            CollisionData = coll;
            EntityId = gameObject.GetComponent<Entity>().EntityId;
            Publish(this);
        }

        public Collision2D CollisionData { get; set; }
    }
}