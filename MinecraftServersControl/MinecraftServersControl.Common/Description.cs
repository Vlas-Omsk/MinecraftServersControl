using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MinecraftServersControl.Common
{
    public static class Description
    {
        public static string GetEnumValueDescription(Enum obj)
        {
            var type = obj.GetType();
            var field = type.GetFields(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(x =>
                {
                    var constantValue = x.GetRawConstantValue();
                    var c = Convert.ChangeType(obj, constantValue.GetType());
                    return constantValue.Equals(c);
                });

            return field?.GetCustomAttribute<DescriptionAttribute>(false)?.Description;
        }

        public static string Get(ICustomAttributeProvider member)
        {
            return ((DescriptionAttribute[])member.GetCustomAttributes(typeof(DescriptionAttribute), false)).FirstOrDefault()?.Description;
        }
    }
}
