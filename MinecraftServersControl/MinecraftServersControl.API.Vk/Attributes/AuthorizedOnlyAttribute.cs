using System;

namespace MinecraftServersControl.API.Vk
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AuthorizedOnlyAttribute : Attribute
    {
    }
}
