using System;

namespace uFrame.Editor.Graphs.Data
{
    public interface IJsonTypeResolver
    {
        Type FindType(string clrTypeString);
    }
}