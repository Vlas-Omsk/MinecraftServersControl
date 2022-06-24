using System;

namespace MinecraftServersControl.Remote.Core.DTO
{
    public enum RemoteErrorCode
    {
        None = 0,

        ServerAlredyStarted = 2,
        ServerAlredyStopped = 3,
        ServerNotFound = 4,
        CantStartServer = 5
    }
}
