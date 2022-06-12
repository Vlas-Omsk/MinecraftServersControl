using System;

namespace MinecraftServersControl.Remote.DTO
{
    public enum RemoteResultCode
    {
        Success = 0,
        Verify = 1,
        ServerStarted = 2,
        ServerStopped = 3,
        ServerNotFound = 4,
        CantStartServer = 5,
        ServerOutput = 6
    }
}
