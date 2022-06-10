using System;
using System.Reflection;

namespace MinecraftServersControl.Common
{
    public sealed class MethodAttribute<T> where T : Attribute
    {
        public MethodInfo Method { get; }
        public T Attribute { get; }

        public MethodAttribute(MethodInfo method, T attribute)
        {
            Method = method;
            Attribute = attribute;
        }
    }
}
