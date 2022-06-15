using System;

namespace MinecraftServersControl.Remote.DTO
{
    public enum RemoteErrorCode
    {
        None = 0,

        ServerStarted = 2,
        ServerStopped = 3,
        ServerNotFound = 4,
        CantStartServer = 5
    }
}
