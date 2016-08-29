using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine;

namespace uFrame.ECS.Actions
{
    [ActionLibrary, uFrameCategory("Vector3")]
    public static class Vector3Library
    {
        public static Vector3 Multiply(Vector3 a, float b)
        {
            return a*b;
        }

        public static Vector3 Add(Vector3 a, Vector3 b)
        {
            return a + b;
        }

        public static Vector3 Subtract(Vector3 a, Vector3 b)
        {
            return a - b;
        }
        [ActionTitle("Translate With Time")]
        public static void TranslateWithTime(EcsComponent component, Vector3 direction)
        {
            component.transform.position += Time.deltaTime*direction;
        }

        [ActionTitle("Translate")]
        public static void Translate(EcsComponent component, float x, float y, float z)
        {
            component.transform.position += new Vector3(x, y, z);
        }

        [ActionTitle("Get Indices")]
        public static void GetIndices(Vector3 vector,out float x, out float y, out float z)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }
        [ActionTitle("Get X")]
        public static float GetX(Vector3 vector)
        {
            return vector.x;
        }
        
        [ActionTitle("Get Y")]
        public static float GetY(Vector3 vector)
        {
            return vector.y;
        }
        
        [ActionTitle("Get Z")]
        public static float GetZ(Vector3 vector)
        {
            return vector.z;
        }

        [ActionTitle("Create Vector3")]
        public static Vector3 Create(float x, float y, float z)
        {
            return new Vector3(x,y,z);
        }
    }


}