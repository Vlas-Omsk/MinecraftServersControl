using MinecraftServersControl.API.Schema;
using System;

namespace MinecraftServersControl.API
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
