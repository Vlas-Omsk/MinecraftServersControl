using System;

namespace MinecraftServersControl.API.Schema
{
    public enum WebSocketResponseCode : int
    {
        Success = 0,
        DataError = 1,
        InternalServerError = 2,
        InvalidCode = 3,
        InvalidState = 4,
        CoreError = 5,
    }
}
