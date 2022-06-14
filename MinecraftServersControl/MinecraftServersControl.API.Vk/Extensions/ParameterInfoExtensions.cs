using System;
using System.Reflection;

namespace MinecraftServersControl.API.Vk
{
    internal static class ParameterInfoExtensions
    {
        public static bool IsParamArray(this ParameterInfo parameter)
        {
            return parameter.GetCustomAttribute<ParamArrayAttribute>() != null;
        }
    }
}
