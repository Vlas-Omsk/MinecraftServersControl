using MinecraftServersControl.Core.DTO;
using System;

namespace MinecraftServersControl.API.Schema
{
    public enum WebSocketRequestCode : int
    {
        // AuthApiService
        [DataType(typeof(UserDTO))]
        SignIn = 1,
        [DataType(typeof(Guid))]
        Restore = 2,

        // Common
        [DataType(typeof(Guid))]
        Auth = 3,

        // ServersApiService
        [DataType(null)]
        GetServers = 4,
    }
}
