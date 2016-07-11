using System;

namespace uFrame.Kernel.Serialization
{
    public interface ITypeResolver
    {
        Type GetType(string name);
        string SetType(Type type);
        object CreateInstance(string name, string identifier);
    }
}