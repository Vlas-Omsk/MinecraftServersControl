using System;

namespace MinecraftServersControl.Core.DTO
{
    public enum ErrorCode : int
    {
        None = 0,

        UserNotFound = 1,
        SessionExpired = 2,
        ComputerNotFound = 4,
        ServerStarted = 5,
        ServerStopped = 6,
        ComputerStarted = 7,
        ComputerStopped = 8,
        ServerNotFound = 10,
        CantStartServer = 11,
    }
}
