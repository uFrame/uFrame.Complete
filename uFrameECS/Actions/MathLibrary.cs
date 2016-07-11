using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uFrame.Attributes;
using uFrame.ECS;

namespace uFrame.ECS.Actions
{

    [ActionLibrary,uFrameCategory("Math")]
    public static class MathLibrary
    {

        [ActionTitle("Multiply")]
        public static void Multiply(float a, float b, out float result)
        {
            result = a*b;
        }



    }
}
