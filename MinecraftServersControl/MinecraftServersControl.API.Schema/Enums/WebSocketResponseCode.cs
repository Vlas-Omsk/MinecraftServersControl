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
        ComputerStarted = 6,
        ComputerStopped = 7,
        ServerStarted = 8,
        ServerStopped = 9,
        ServerOutput = 10,
        SessionRemoved = 11
    }
}
