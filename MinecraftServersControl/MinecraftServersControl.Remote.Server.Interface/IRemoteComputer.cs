using MinecraftServersControl.Remote.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core
{
    public interface IRemoteComputer
    {
        Task<RemoteResult<IEnumerable<ServerInfoDTO>>> GetInfo(Guid computerId);
        Task<RemoteResult<string>> GetOutput(Guid computerId, Guid serverId);
        Task<RemoteResult> Input(Guid computerId, ServerInputDTO serverInput);
        Task<RemoteResult> Start(Guid computerId, Guid serverId);
        Task<RemoteResult> Terminate(Guid computerId, Guid serverId);
    }
}
