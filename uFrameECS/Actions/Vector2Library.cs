using uFrame.Attributes;
using UnityEngine;

namespace uFrame.ECS.Actions
{
    [ActionLibrary, uFrameCategory("Vector2")]
    public static class Vector2Library
    {

        public static Vector2 Multiply(Vector2 a, float b)
        {
            return a * b;
        }

        public static Vector2 Add(Vector2 a, Vector2 b)
        {
            return a + b;
        }

        public static Vector2 Subtract(Vector2 a, Vector2 b)
        {
            return a - b;
        }

        [ActionTitle("Get Indices")]
        public static void GetIndices(Vector2 vector, out float x, out float y)
        {
            x = vector.x;
            y = vector.y;
        }
        [ActionTitle("Get X")]
        public static float GetX(Vector2 vector)
        {
            return vector.x;
        }

        [ActionTitle("Get Y")]
        public static float GetY(Vector2 vector)
        {
            return vector.y;
        }


        [ActionTitle("Create Vector2")]
        public static Vector2 Create(float x, float y)
        {
            return new Vector2(x, y);
        }
    }
    [ActionLibrary, uFrameCategory("Float")]
    public static class FloatLibrary
    {

        public static float Multiply(float a, float b)
        {
            return a * b;
        }

        public static float Add(float a, float b)
        {
            return a + b;
        }

        public static float Subtract(float a, float b)
        {
            return a - b;
        }
        public static float Divide(float a, float b)
        {
            return a / b;
        }
       
    }
    [ActionLibrary, uFrameCategory("Int")]
    public static class IntLibrary
    {
        public static int Increment(int a)
        {
            return a+1;
        }
        public static int Decrement(int a)
        {
            return a-1;
        }
        public static int Multiply(int a, int b)
        {
            return a * b;
        }

        public static int Add(int a, int b)
        {
            return a + b;
        }

        public static int Subtract(int a, int b)
        {
            return a - b;
        }

        public static int Divide(int a, int b)
        {
            return a / b;
        }

    }
}