using MinecraftServersControl.Remote.Schema;
using System;

namespace MinecraftServersControl.Remote.Client
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class WebSocketRequestAttribute : Attribute
    {
        public WebSocketRequestCode Code { get; }

        public WebSocketRequestAttribute(WebSocketRequestCode code)
        {
            Code = code;
        }
    }
}
