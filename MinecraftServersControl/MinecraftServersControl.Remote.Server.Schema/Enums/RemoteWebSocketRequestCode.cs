using System;

namespace MinecraftServersControl.Remote.Server.Schema
{
    public enum RemoteWebSocketRequestCode : int
    {
        GetInfo = 1,
        GetOutput = 2,
        Start = 3,
        Terminate = 4,
        Input = 5,
    }
}
