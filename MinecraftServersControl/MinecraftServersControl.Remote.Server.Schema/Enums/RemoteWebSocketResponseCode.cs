using System;

namespace MinecraftServersControl.Remote.Server.Schema
{
    public enum RemoteWebSocketResponseCode : int
    {
        Success = 0,

        Verify = 1,
        CoreError = 2,
        ServerOutput = 3,
        ServerStarted = 4,
        ServerStopped = 5,
    }
}
