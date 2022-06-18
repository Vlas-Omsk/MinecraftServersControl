using MinecraftServersControl.Common;
using System;
using System.Linq;
using System.Reflection;

namespace MinecraftServersControl.API.Vk
{
    internal static class FormatHelper
    {
        public static string ToStringYesNo(bool value)
        {
            return value ? "да" : "нет";
        }

        public static string ToStringOnOff(bool value)
        {
            return value ? "включен" : "отключен";
        }

        public static string ToStringCommandShort(MethodInfo method)
        {
            var service = ToStringCommand(method);
            var description = Description.Get(method);
            var parameters = string.Join(' ', method.GetParameters().Select(x => $"[{ToStringParameterShort(x)}]"));

            if (description != null)
                description = " - " + description;

            return $"{service} {parameters}{description}";
        }

        public static string ToStringCommandLong(MethodInfo method)
        {
            var service = ToStringCommand(method);
            var description = Description.Get(method);
            var parameters = string.Join("\r\n", method.GetParameters().Select(x => ToStringParameterLong(x)));

            if (description != null)
                description = " - " + description;

            return $"{service}{description}\r\n{parameters}";
        }

        public static string ToStringCommand(MethodInfo method)
        {
            var result = "";

            var serviceAttribute = method.DeclaringType.GetCustomAttribute<ServiceAttribute>();
            if (serviceAttribute != null)
                result += string.Join(' ', serviceAttribute.Segments) + ' ';
            result += string.Join(' ', method.GetCustomAttribute<CommandAttribute>().Segments);

            return result;
        }

        public static string ToStringParameterShort(ParameterInfo parameter)
        {
            return $"{(parameter.GetCustomAttribute<ParamArrayAttribute>() == null ? null : "много ")}{parameter.ParameterType.Name}: {parameter.GetCustomAttribute<CommandParameterAttribute>().Name}";
        }

        public static string ToStringParameterLong(ParameterInfo parameter)
        {
            var description = Description.Get(parameter);

            if (description != null)
                description = " - " + description;

            return $"{ToStringParameterShort(parameter)}{description}";
        }
    }
}
