using System;
using System.Collections.Generic;

namespace uFrame.Editor.Graphs.Data.Types
{
    public class DefaultMemberInfo : IMemberInfo
    {
        public string MemberName { get; set; }
        public ITypeInfo MemberType { get; set; }
        public IEnumerable<Attribute> GetAttributes()
        {
            yield break;
        }
    }
}