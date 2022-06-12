using System;

namespace MinecraftServersControl.Core.DTO
{
    public enum ResultCode : int
    {
        Success = 0,

        UserNotFound = 1,
        SessionExpired = 2,
        AuthorizationFromAnotherPlace = 3,
        ComputerNotFound = 4,
        ServerStarted = 5,
        ServerStopped = 6,
        ComputerStarted = 7,
        ComputerStopped = 8,
        ServerOutput = 9,
        ServerNotFound = 10,
        CantStartServer = 11
    }
}
