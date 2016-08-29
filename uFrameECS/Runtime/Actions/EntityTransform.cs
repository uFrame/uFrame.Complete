using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine;

namespace uFrame.ECS.Actions
{
    [ActionLibrary, uFrameCategory("Transform","Entity")]
    public static class EntityTransform
    {
        [ActionTitle("Set Position")]
        public static void SetPosition(Entity entity, Vector3 position)
        {
            entity.transform.position = position;
        }

        [ActionTitle("Set Rotation")]
        public static void SetRotation(Entity entity, Vector3 rotation)
        {
            entity.transform.rotation = Quaternion.Euler(rotation);
        }
        [ActionTitle("Set Local Position")]
        public static void SetLocalPosition(Entity entity, Vector3 position)
        {
            entity.transform.localPosition = position;
        }

        [ActionTitle("Set Local Rotation")]
        public static void SetLocalRotation(Entity entity, Vector3 rotation)
        {
            entity.transform.localRotation = Quaternion.Euler(rotation);
        }

        [ActionTitle("Set Scale")]
        public static void SetScale(Entity entity, Vector3 scale)
        {
            entity.transform.localScale = scale;
        }


        [ActionTitle("Get Position")]
        public static Vector3 GetPosition(Entity entity)
        {
            return entity.transform.position;
        }

        [ActionTitle("Get Rotation")]
        public static Vector3 GetRotation(Entity entity)
        {
            return entity.transform.eulerAngles;
        }
        [ActionTitle("Get Local Position")]
        public static Vector3 GetLocalPosition(Entity entity)
        {
            return entity.transform.localPosition;
        }

        [ActionTitle("Get Local Rotation")]
        public static Vector3 GetLocalRotation(Entity entity)
        {
            return entity.transform.localEulerAngles;
        }

        [ActionTitle("Get Local Scale")]
        public static Vector3 GetLocalScale(Entity entity)
        {
            return entity.transform.localScale;
        }
    }
}