using MinecraftServersControl.Remote.DTO;
using System;

namespace MinecraftServersControl.Remote.Server
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class WebSocketResponseAttribute : Attribute
    {
        public ResultCode Code { get; }

        public WebSocketResponseAttribute(ResultCode code)
        {
            Code = code;
        }
    }
}
