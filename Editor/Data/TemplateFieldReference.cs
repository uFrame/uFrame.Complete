using System.Reflection;

namespace uFrame.Architect.Editor.Data
{
    public class TemplateFieldReference : TemplateReference
    {
        public FieldInfo FieldInfo
        {
            get { return MemberInfo as FieldInfo; }
        }

    }
}