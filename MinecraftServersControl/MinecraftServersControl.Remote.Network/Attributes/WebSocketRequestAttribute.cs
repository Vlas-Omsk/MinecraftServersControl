using MinecraftServersControl.Remote.Server.Schema;
using System;

namespace MinecraftServersControl.Remote.Client
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class WebSocketRequestAttribute : Attribute
    {
        public RemoteWebSocketRequestCode Code { get; }

        public WebSocketRequestAttribute(RemoteWebSocketRequestCode code)
        {
            Code = code;
        }
    }
}
