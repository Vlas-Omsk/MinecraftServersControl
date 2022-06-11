using System;

namespace MinecraftServersControl.Remote.Schema
{
    public enum WebSocketRequestCode
    {
        GetInfo = 1,
        GetOutput = 2,
        Start = 3,
        Terminate = 4,
    }
}
