﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MinecraftServersControl.Common
{
    public static class TypeExtensions
    {
        public static IEnumerable<MethodAttribute<T>> GetMethodsWithAttribute<T>(this Type self) where T : Attribute
        {
            return self
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(x => new MethodAttribute<T>(x, x.GetCustomAttribute<T>()))
                .Where(x => x.Attribute != null);
        }
    }
}
