using System;
using uFrame.Attributes;

namespace uFrame.ECS.Actions
{
    [ActionLibrary, uFrameCategory("Set")]
    public static class SetLibrary
    {
        [ActionTitle("Set Value")]
        public static void SetValue( ref object a, object value)
        {
            if (a == null) throw new ArgumentNullException("a");
            a = value;
        }
    }
}