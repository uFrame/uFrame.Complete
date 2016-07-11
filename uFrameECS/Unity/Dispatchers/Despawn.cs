using uFrame.Attributes;
using uFrame.ECS.Actions;
using uFrame.ECS.Components;

namespace uFrame.ECS.UnityUtilities
{
    [ActionTitle("Spawn")]
    public class Despawn : UFAction
    {
        [In] public int EntityId;
        [In] public Entity Entity;

        public override void Execute()
        {
            System.Publish(new DespawnEntity()
            {
                Entity = Entity,
                EntityId = EntityId
            });
        }
    }
}