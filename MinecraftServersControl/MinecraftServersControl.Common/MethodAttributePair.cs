using System;
using System.Reflection;

namespace MinecraftServersControl.Common
{
    public sealed class MethodAttributePair<T> where T : Attribute
    {
        public MethodInfo Method { get; }
        public T Attribute { get; }

        public MethodAttributePair(MethodInfo method, T attribute)
        {
            Method = method;
            Attribute = attribute;
        }
    }
}
