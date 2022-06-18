using System;

namespace MinecraftServersControl.Core.DTO
{
    public enum ErrorCode : int
    {
        None = 0,

        UserNotFound = 1,
        SessionExpired = 2,
        ComputerNotFound = 4,
        ServerAlredyStarted = 5,
        ServeAlredyrStopped = 6,
        ComputerAlredyStarted = 7,
        ComputerAlredyStopped = 8,
        ServerNotFound = 10,
        CantStartServer = 11,
    }
}
