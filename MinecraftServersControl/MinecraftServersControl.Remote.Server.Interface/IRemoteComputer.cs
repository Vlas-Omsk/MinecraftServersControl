using MinecraftServersControl.Remote.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core
{
    public interface IRemoteComputer
    {
        Task<RemoteResult<IEnumerable<ServerInfoDTO>>> GetInfo();
        Task<RemoteResult<string>> GetOutput(Guid serverId);
        Task<RemoteResult> Input(ServerInputDTO serverInput);
        Task<RemoteResult> Start(Guid serverId);
        Task<RemoteResult> Terminate(Guid serverId);
    }
}
