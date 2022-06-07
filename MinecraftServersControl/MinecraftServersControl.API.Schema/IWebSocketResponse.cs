using MinecraftServersControl.Core.DTO;
using System;

namespace MinecraftServersControl.API.Schema
{
    public interface IWebSocketResponse
    {
        int RequestId { get; }
        WebSocketResponseCode Code { get; }
        string ErrorMessage { get; }
        IResult Result { get; }
    }
}
