using System;

namespace uFrame.MVVM.Attributes
{
    public class UFRequireInstanceMethod : Attribute
    {
        public UFRequireInstanceMethod(string canmovetochanged)
        {
            MethodName = canmovetochanged;
        }

        public string MethodName { get; set; }
    }
}