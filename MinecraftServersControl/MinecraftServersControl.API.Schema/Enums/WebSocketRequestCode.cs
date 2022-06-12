using System;

namespace MinecraftServersControl.API.Schema
{
    public enum WebSocketRequestCode : int
    {
        Auth = 1,
        GetServers = 2,
        StartServer = 3,
        TerminateServer = 4,
        GetOutput = 5,
        Input = 6
    }
}
