using System;
using System.Collections.Generic;

namespace uFrame.Editor.Graphs.Data.Types
{
    public interface IMemberInfo
    {
        string MemberName { get; }
        ITypeInfo MemberType { get; }
        IEnumerable<Attribute> GetAttributes();
    }

    public interface IMethodMemberInfo : IMemberInfo
    {
        string MethodIdentifier { get; }

        IEnumerable<IMemberInfo> GetParameters();
    }
}