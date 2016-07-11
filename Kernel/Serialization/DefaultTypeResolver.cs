using System;

namespace uFrame.Kernel.Serialization
{
    public class DefaultTypeResolver : ITypeResolver
    {
        public Type GetType(string name)
        {
            return Type.GetType(name);
        }

        public string SetType(Type type)
        {
            return type.AssemblyQualifiedName;
        }

        public virtual object CreateInstance(string name, string identifier)
        {

            return Activator.CreateInstance(GetType(name));
        }
    }
}