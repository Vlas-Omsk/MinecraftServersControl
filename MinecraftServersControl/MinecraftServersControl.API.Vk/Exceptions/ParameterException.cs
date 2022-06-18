using System;
using System.Reflection;

namespace MinecraftServersControl.API.Vk
{
    internal sealed class ParameterException : Exception
    {
        public ParameterInfo Parameter { get; }

        public ParameterException(ParameterInfo parameter, Exception innerException) : base(null, innerException)
        {
            Parameter = parameter;
        }
    }
}
