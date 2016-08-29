using System;

/// <summary>
/// Custom Event Attributes of uFrame
/// </summary>
namespace uFrame.Attributes
{
    public class uFrameEvent : Attribute
    {
        public string Title { get; set; }

        public uFrameEvent() {}

        public uFrameEvent(string title)
        {
            Title = title;
        }
    }

    public class UFrameEventDispatcher : uFrameEvent
    {
        public UFrameEventDispatcher()
        {
        }

        public UFrameEventDispatcher(string title) : base(title)
        {
        }
    }

    public class SystemUFrameEvent : uFrameEvent
    {
        public SystemUFrameEvent(string title, string systemMethodName) : base(title)
        {
            SystemMethodName = systemMethodName;
        }

        public string SystemMethodName { get; set; }
    }

    public class uFrameCategory : Attribute
    {
        public string[] Title { get; set; }
        public uFrameCategory(params string[] title)
        {
            Title = title;
        }
    }

    public class uFrameEventMapping : Attribute
    {
        public string Title { get; set; }

        public uFrameEventMapping(string title)
        {
            Title = title;
        }
    }
}

//public enum AutoFillType
//{
//    None,
//    NameOnly,
//    NameOnlyWithBackingField,
//    NameAndType,
//    NameAndTypeWithBackingField
//}