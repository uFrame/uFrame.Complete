using System;
using uFrame.Attributes;

namespace uFrame.ECS.Editor
{
    public class EventFieldInfo
    {
        private string _title;
        public Type Type { get; set; }
        public string Name { get; set; }

        public string Title
        {
            get
            {
                if (!string.IsNullOrEmpty(_title)) return _title;
                if (Attribute == null) return Name;
                return Attribute.Title;
            }
            set { _title = value; }
        }

        public bool IsProperty { get; set; }
        public uFrameEventMapping Attribute { get; set; }
    }
}