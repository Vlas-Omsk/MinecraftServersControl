using System;

namespace MinecraftServersControl.API.Schema
{
    public enum ResponseCode : int
    {
        Success = 0,
        DataError = 1,
        InvalidState = 2,
        InternalServerError = 3,
        InvalidCode = 4,
    }
}
