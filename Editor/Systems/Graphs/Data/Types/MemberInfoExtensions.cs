using System.Linq;

namespace uFrame.Editor.Graphs.Data.Types
{
    public static class MemberInfoExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this IMemberInfo memberInfo)
        {
            return memberInfo.GetAttributes().OfType<TAttribute>().FirstOrDefault();
        }
        public static bool HasAttribute<TAttribute>(this IMemberInfo memberInfo)
        {
            return memberInfo.GetAttributes().OfType<TAttribute>().Any();
        }
    }
}