using System;

namespace MinecraftServersControl.API.Schema
{
    public interface IWebSocketRequest
    {
        int Id { get; }
        WebSocketRequestCode Code { get; }
        object Data { get; }
    }
}
