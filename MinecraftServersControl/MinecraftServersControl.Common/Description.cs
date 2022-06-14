using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MinecraftServersControl.Common
{
    public static class Description
    {
        public static string Get(object obj)
        {
            var type = obj.GetType();
            MemberInfo member;

            if (type.IsEnum)
                member = type.GetFields(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(x =>
                    {
                        var constantValue = x.GetRawConstantValue();
                        var c = Convert.ChangeType(obj, constantValue.GetType());
                        return constantValue.Equals(c);
                    });
            else
                member = type;

            return member.GetCustomAttribute<DescriptionAttribute>(false)?.Description;
        }
    }
}
