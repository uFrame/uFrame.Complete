using System;
using System.Collections;
using System.Globalization;
using uFrame.Actions.Attributes;
using uFrame.Attributes;
using uFrame.ECS;
using UnityEngine;

namespace uFrame.ECS.Actions
{
    [ActionLibrary, uFrameCategory("Condition")]
    public static class Comparisons
    {
        [ActionTitle("Is True")]
        [ActionDescription("Compare any incoming value with true")]
        public static void IsTrue(
            [Description("Value to compare")] bool value,
            [Description("Connect any action to be invoked, if value is true.")] Action yes,
            [Description("Connect any action to be invoked, if value is false.")] Action no)
        {
            if (value)
            {
                if (yes != null) yes();

            }

                else
                {
                    if (no != null) no();
                }
        }

        [ActionTitle("Compare Floats")]
        [ActionDescription("Compare any two floats")]

        public static bool CompareFloats(float a, float b)
        {
            return a == b;
        }

        [ActionTitle("Less Than")]
        [ActionDescription("Compare any two floats and continue execution with a certain branch")]
        public static bool LessThan(float a, float b,
            [Description("Invoked if a is less than b")] Action yes,
            [Description("Invoked if a is equal or greater than b")] Action no)
        {
            if (a < b)
            {
                if (yes != null) yes();
                return true;
            }
            else
            {
                if (no != null) no();
            }

            return false;
        }

        [ActionTitle("Less Than Or Equal")]
        [ActionDescription("Compare any two floats and continue execution with a certain branch")]
        public static bool LessThanOrEqual(float a, float b,
            [Description("Invoked if a is less than or equal to b")] Action yes,
            [Description("Invoked if a is greater than b")] Action no)
        {
            if (a <= b)
            {
                if (yes != null) yes();
                return true;
            }
            else
            {
                if (no != null) no();
            }

            return false;
        }

        [ActionTitle("Greater Than")]
        [ActionDescription("Compare any two floats and continue execution with a certain branch")]
        public static bool GreaterThan(float a, float b,
            [Description("Invoked if a is greater than b")] Action yes,
            [Description("Invoked if a is less than or equal to b")] Action no)
        {
            if (a > b)
            {
                if (yes != null) yes();
                return true;
            }
            else
            {
                if (no != null) no();
            }

            return false;
        }

        [ActionTitle("Greater Than Or Equal")]
        [ActionDescription("Compare any two floats and continue execution with a certain branch")]
        public static bool GreaterThanOrEqual(float a, float b,
            [Description("Invoked if a is greater than or equal to b")] Action yes,
            [Description("Invoked if a is less than b")] Action no)
        {
            if (a >= b)
            {
                if (yes != null) yes();
                return true;
            }
            else
            {
                if (no != null) no();
            }

            return false;
        }

        [ActionTitle("Equal")]
        [ActionDescription("Compare any two floats and continue execution with a certain branch")]
        public static bool AreEqual(object a, object b,
            [Description("Invoked if a equals b")] Action yes,
            [Description("Invoked if a is not equal to b")] Action no)
        {
            var result = false;
            if ((a == null || b == null))
            {
                result = a == b;
                if (yes != null) yes();
            }
            else
            {
                if (a.Equals(b))
                {
                    if (yes != null) yes();
                }
                else
                {
                    if (no != null)
                    {
                        no();
                    }
                }
            }

            return result;
        }

    }

    [ActionLibrary, uFrameCategory("Components")]
    public static class UnityLibrary
    {

        [ActionTitle("Get Unity Component")]
        public static Type GetUnityComponent<Type>(GameObject go, MonoBehaviour component)
        {
            if (component == null)
                return go.GetComponent<Type>();
            return component.GetComponent<Type>();
        }
        [ActionTitle("Get Rigidbody")]
        public static Rigidbody GetRigidbody(GameObject go, MonoBehaviour component)
        {
            return GetUnityComponent<Rigidbody>(go, component);
        }
        [ActionTitle("Get Rigidbody2D")]
        public static Rigidbody2D GetRigidbody2D(GameObject go, MonoBehaviour component)
        {
            return GetUnityComponent<Rigidbody2D>(go, component);
        }
        [ActionTitle("Get Collider 2D")]
        public static Collider2D GetCollider2D(GameObject go, MonoBehaviour component)
        {
            return GetUnityComponent<Collider2D>(go, component);
        }
        [ActionTitle("Get Collider")]
        public static Collider GetCollider(GameObject go, MonoBehaviour component)
        {
            return GetUnityComponent<Collider>(go, component);
        }
        [ActionTitle("Get Camera")]
        public static Camera GetCamera(GameObject go, MonoBehaviour component)
        {
            return GetUnityComponent<Camera>(go, component);
        }
        [ActionTitle("Get Main Camera")]
        public static Camera GetMainCamera()
        {
            return Camera.main;
        }
    }

    [ActionLibrary, uFrameCategory("Loops")]
    public static class LoopsLibrary
    {
        //[ActionTitle("Loop Collection")]
        //public static void LoopCollection(
        //    [Description("A list which you are going to iterate over.")]IList list,
        //    [Description("On each iteration, item will be set to the corresponding element from the list.")] out object item,
        //    [Description("Connect an action, which will be invoked on each iteration.")] Action next)
        //{
        //    item = null;
        //    for (var i = 0; i < list.Count; i++)
        //    {
        //        item = list[i];
        //        next();
        //    }
        //}
    }


    [ActionLibrary, uFrameCategory("Input")]
    public static class InputLibrary
    {
        [ActionTitle("Is Key Down"), ActionDescription("Check if key is down")]
        public static bool IsKeyDown(KeyCode key, Action yes, Action no)
        {
            var result = Input.GetKeyDown(key);
            if (result)
            {
                if (yes != null) yes();
            }
            else
            {
                if (no != null)
                    no();
            }
            return result;
        }
        [ActionTitle("Is Key"), ActionDescription("Check if key is hold")]
        public static bool IsKey(KeyCode key, Action yes, Action no)
        {
            var result = Input.GetKey(key);
            if (result)
            {
                if (yes != null) yes();

            }

                else
                {
                    if (no != null)
                        no();
                }
            return Input.GetKey(key);
        }
        [ActionTitle("Is Key Up"), ActionDescription("Check if key is up")]
        public static bool IsKeyUp(KeyCode key, Action yes, Action no)
        {
            var result = Input.GetKeyUp(key);
            if (result)
            {
                if (yes != null) yes();
            }

                else
                {
                    if (no != null)
                        no();
                }
            return result;
        }
    }

    [ActionLibrary, uFrameCategory("Rigidbody")]
    public static class RigidbodyLibrary
    {
        [ActionTitle("Set Velocity")]
        public static void SetVelocity(Rigidbody rigidBody, float x, float y, float z)
        {
            rigidBody.velocity = new Vector3(x, y, z);
        }
        [ActionTitle("Set Velocity With Speed")]
        public static void SetVelocityWithSpeed(Rigidbody rigidBody, float x, float y, float z, float speed)
        {
            rigidBody.velocity = new Vector3(x, y, z) * speed;
        }
        [ActionTitle("Set Rigidbody Position (Floats)")]
        public static void SetRigidbodyPosition(Rigidbody rigidBody, float x, float y, float z)
        {
            rigidBody.position = new Vector3(x, y, z);
        }

        [ActionTitle("Set Rigidbody Position (Vector)")]
        public static void SetRigidbodyPosition(Rigidbody rigidBody, Vector3 vector)
        {
            rigidBody.position = vector;
        }

        [ActionTitle("Set Rigidbody Rotation")]
        public static void SetRigidbodyRotation(Rigidbody rigidBody, float x, float y, float z)
        {
            rigidBody.rotation = Quaternion.Euler(x, y, z);
        }
    }

    [ActionLibrary, uFrameCategory("Time")]
    public static class TimeLibrary
    {
        [ActionTitle("Get Time")]
        public static float GetTime()
        {
            return Time.time;
        }
        [ActionTitle("Get Delta Time")]
        public static float GetDeltaTime()
        {
            return Time.deltaTime;
        }
        [ActionTitle("Get Fixed Time")]
        public static float GetFixedTime()
        {
            return Time.fixedTime;
        }
        [ActionTitle("Get Fixed Delta Time")]
        public static float GetFixedDeltaTime()
        {
            return Time.fixedDeltaTime;
        }
    }

    [ActionLibrary, uFrameCategory("Convert")]
    public static class Converter
    {
        [ActionTypeConverter()]
        public static string ConvertToString(object obj)
        {
            return obj.ToString();
        }
        [ActionTypeConverter()]
        public static int FloatToInt(float @in)
        {
            return (int) @in;
        }
        [ActionTypeConverter()]
        public static float IntToFloat(int @in)
        {
            return @in;
        }
        [ActionTypeConverter()]
        public static int StringToInt(string @in, NumberStyles style)
        {
            return int.Parse(@in,style);
        }
        [ActionTypeConverter()]
        public static float StringToFloat(string @in, NumberStyles style)
        {
            return float.Parse(@in, style);
        }
        [ActionTypeConverter()]
        public static DateTime StringToDateTime(string @in)
        {
            return DateTime.Parse(@in);
        }
        [ActionTypeConverter()]
        public static bool StringToBool(string @in)
        {
            return bool.Parse(@in);
        }
    }

}



namespace uFrame.Actions.Attributes
{



}
