using MinecraftServersControl.Remote.DTO;
using System;

namespace MinecraftServersControl.Remote.Server
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class WebSocketResponseAttribute : Attribute
    {
        public RemoteResultCode Code { get; }

        public WebSocketResponseAttribute(RemoteResultCode code)
        {
            Code = code;
        }
    }
}
