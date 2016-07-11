namespace uFrame.Attributes
{
    public class SystemUFrameEvent : uFrameEvent
    {

        public SystemUFrameEvent(string title, string systemMethodName)
            : base(title)
        {
            SystemMethodName = systemMethodName;
        }

        public string SystemMethodName { get; set; }
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

