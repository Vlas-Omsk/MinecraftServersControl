using MinecraftServersControl.Remote.Server.Schema;
using System;

namespace MinecraftServersControl.Remote.Server
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class WebSocketResponseAttribute : Attribute
    {
        public RemoteWebSocketResponseCode Code { get; }

        public WebSocketResponseAttribute(RemoteWebSocketResponseCode code)
        {
            Code = code;
        }
    }
}
