using System;

namespace uFrame.MVVM.Attributes
{
    public class UFGroup : Attribute
    {
        public UFGroup(string viewModelProperties)
        {
            Name = viewModelProperties;
        }

        public string Name { get; set; }
    }

}
