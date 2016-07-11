using System.Reflection;

namespace uFrame.Architect.Editor.Data
{
    public class TemplateEventReference : TemplateReference
    {
        public EventInfo EventInfo
        {
            get { return MemberInfo as EventInfo; }
        }
    }
}