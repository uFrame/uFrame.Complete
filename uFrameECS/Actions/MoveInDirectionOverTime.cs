using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine;

namespace uFrame.ECS.Actions
{
    [ActionTitle("Movement/Move Over Time")]
    public class MoveInDirectionOverTime : UFAction
    {
        [In] public EcsComponent Component;
        [In] public float Speed;
        [In] public Vector3 Direction;

        public override void Execute()
        {
            Component.CachedTransform.position += (Direction*Speed)*Time.deltaTime;

        }
    }
}