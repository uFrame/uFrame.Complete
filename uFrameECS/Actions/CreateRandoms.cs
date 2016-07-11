using uFrame.Attributes;
using UnityEngine;

namespace uFrame.ECS.Actions
{
    [ActionLibrary, uFrameCategory("Random")]
    public static class CreateRandoms
    {
        [ActionTitle("Random Inside Unit Sphere")]
        public static Vector3 GetInsideSphere()
        {
            return Random.insideUnitSphere;
        }
        [ActionTitle("Random Inside Unit Sphere")]
        public static Vector2 GetInsideCircle()
        {
            return Random.insideUnitCircle;
        }
        [ActionTitle("Random Vector3")]
        public static Vector3 RandomVector3(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            return new Vector3(
                UnityEngine.Random.Range(minX, maxX),
                UnityEngine.Random.Range(minY, maxY),
                UnityEngine.Random.Range(minZ, maxZ)
                );
        }
        [ActionTitle("Random Vector2")]
        public static Vector2 RandomVector2(float minX, float maxX, float minY, float maxY)
        {
            return new Vector2(
                UnityEngine.Random.Range(minX, maxX),
                UnityEngine.Random.Range(minY, maxY)
                );
        }
        [ActionTitle("Random Float")]
        public static float RandomFloat(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
        [ActionTitle("Random Int")]
        public static int RandomInt(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }
        [ActionTitle("Random Bool")]
        public static bool RandomBool()
        {
            return UnityEngine.Random.Range(0, 2) == 1;
        }
    }
}