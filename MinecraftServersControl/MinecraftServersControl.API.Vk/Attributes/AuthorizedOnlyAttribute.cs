using System;

namespace MinecraftServersControl.API.Vk
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class AuthorizedOnlyAttribute : Attribute
    {
    }
}
