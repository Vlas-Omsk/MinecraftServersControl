using MinecraftServersControl.API.Schema;
using System;

namespace MinecraftServersControl.API
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class WebSocketRequestAttribute : Attribute
    {
        public WebSocketRequestCode RequestCode { get; }

        public WebSocketRequestAttribute(WebSocketRequestCode requestCode)
        {
            RequestCode = requestCode;
        }
    }
}
