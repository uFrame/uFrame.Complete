using System;

/// <summary>
/// Custom Action Atrributes of uFrame
/// </summary>
namespace uFrame.Attributes
{
    public class ActionMetaAttribute : Attribute { }

    public class AsyncAction : ActionMetaAttribute { }

    public class ActionTitle : ActionMetaAttribute
    {
        public string Title { get; set; }

        public ActionTitle()
        {
        }

        public ActionTitle(string title)
        {
            Title = title;
        }
    }

    public class ActionTypeConverter : ActionTitle
    {
        public ActionTypeConverter() { }
    }

    public class ActionDescription : ActionMetaAttribute
    {
        public string Description { get; set; }

        public ActionDescription(string description)
        {
            Description = description;
        }
    }

    public class ActionAttribute : ActionMetaAttribute { }

    public class Optional : ActionAttribute { }

    public class Description : ActionAttribute
    {
        public string Text { get; set; }

        public Description(string text)
        {
            Text = text;
        }
    }

    public class FieldDisplayTypeAttribute : ActionAttribute
    {
        public FieldDisplayTypeAttribute(string displayName)
        {
            DisplayName = displayName;
        }

        public FieldDisplayTypeAttribute()
        {
        }

        private bool _isNewLine = true;
        public string DisplayName { get; set; }

        public string ParameterName { get; set; }

        //public int Row { get; set; }

        public FieldDisplayTypeAttribute(string parameterName, string displayName)
        {
            ParameterName = parameterName;
            DisplayName = displayName;
        }

        public FieldDisplayTypeAttribute(string parameterName, string displayName, bool isNewLine)
        {
            ParameterName = parameterName;
            DisplayName = displayName;
            _isNewLine = isNewLine;
        }

        public bool IsNewLine
        {
            get { return _isNewLine; }
            set { _isNewLine = value; }
        }
    }

    public class In : FieldDisplayTypeAttribute
    {
        public In()
        {
        }

        public In(string parameterName, string displayName)
            : base(parameterName, displayName)
        {
        }

        public In(string displayName)
            : base(displayName)
        {
        }

        public In(string parameterName, string displayName, bool isNewLine)
            : base(parameterName, displayName, isNewLine)
        {
        }
    }

    public class Out : FieldDisplayTypeAttribute
    {
        public Out()
        {
        }

        public Out(string parameterName, string displayName)
            : base(parameterName, displayName)
        {
        }

        public Out(string displayName)
            : base(displayName)
        {
        }

        public Out(string parameterName, string displayName, bool isNewLine)
            : base(parameterName, displayName, isNewLine)
        {
        }
    }

    public class ActionTypeSelection : ActionAttribute
    {
        public Type AssignableTo { get; set; }
    }

    public class ActionLibrary : Attribute { }
}
