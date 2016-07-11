using System.Reflection;

namespace uFrame.Architect.Editor.Data
{
    public class TemplatePropertyReference : TemplateReference
    {
        public PropertyInfo PropertyInfo
        {
            get { return MemberInfo as PropertyInfo; }
        }
    }
}