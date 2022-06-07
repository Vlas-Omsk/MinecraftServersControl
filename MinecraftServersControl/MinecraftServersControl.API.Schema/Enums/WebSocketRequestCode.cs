using System;

namespace MinecraftServersControl.API.Schema
{
    public enum WebSocketRequestCode : int
    {
        Auth = 1,
        GetServers = 2,
    }
}
