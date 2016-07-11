using System;
using uFrame.Attributes;
using uFrame.ECS.APIs;
using uFrame.ECS.UnityUtilities;
using UnityEngine;

namespace uFrame.ECS.Actions
{
    [ActionLibrary, uFrameCategory("Destroy", "Component", "Entity")]
    public static class DestroyLibrary
    {
        [ActionTitle("Destroy Component")]
        public static void DestroyComponent(MonoBehaviour behaviour)
        {
            UnityEngine.Object.Destroy(behaviour);
        }
        [ActionTitle("Destroy Entity")]
        public static void DestroyEntity(int entityId, float time)
        {
            UnityEngine.Object.Destroy(EntityService.GetEntityView(entityId).gameObject, time);
        }
        [ActionTitle("Destroy GameObject")]
        public static void DestroyGameObject(GameObject gameObject, float time)
        {
            UnityEngine.Object.Destroy(gameObject, time);
        }
        [ActionTitle("Destroy Timer")]
        public static void DestroyTimer(IDisposable timer)
        {
            timer.Dispose();
        }
        [ActionTitle("Disable Component")]
        public static void DisableComponent(MonoBehaviour behaviour) { behaviour.enabled = false; }

        [ActionTitle("Enable Component")]
        public static void EnableComponent(MonoBehaviour behaviour)
        {
            behaviour.enabled = true;
        }

        [ActionTitle("Remove Component By Type")]
        public static void RemoveComponent(
            GameObject gameObject,
            [ActionTypeSelection(AssignableTo = typeof(IEcsComponent))] Type type
            )
        {
            
        }

    }
}