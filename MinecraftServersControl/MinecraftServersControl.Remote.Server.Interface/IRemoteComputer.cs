using MinecraftServersControl.Remote.DTO;
using MinecraftServersControl.Remote.Schema;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core
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
