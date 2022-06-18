using MinecraftServersControl.Remote.Core.DTO;
using MinecraftServersControl.Remote.Server.Schema;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.Remote.Server.Abstractions
{
    public interface IRemoteComputer
    {
        Task<RemoteWebSocketResponse<ServerInfoDTO[]>> GetInfo();
        Task<RemoteWebSocketResponse<string>> GetOutput(Guid serverKey);
        Task<RemoteWebSocketResponse> Input(ServerInputDTO serverInput);
        Task<RemoteWebSocketResponse> Start(Guid serverId);
        Task<RemoteWebSocketResponse> Terminate(Guid serverId);
    }
}
