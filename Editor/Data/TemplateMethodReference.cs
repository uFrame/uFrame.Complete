using System.Reflection;

namespace uFrame.Architect.Editor.Data
{
    public class TemplateMethodReference : TemplateReference
    {
        public MethodInfo MethodInfo
        {
            get { return MemberInfo as MethodInfo; }
        }
    }
}