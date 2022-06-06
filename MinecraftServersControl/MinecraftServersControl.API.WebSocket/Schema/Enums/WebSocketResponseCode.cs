using System;

namespace MinecraftServersControl.API.Schema
{
    public enum WebSocketResponseCode : int
    {
        Success = 0,
        DataError = 1,
        InvalidState = 2,
        InternalServerError = 3,
        InvalidCode = 4,
    }
}
