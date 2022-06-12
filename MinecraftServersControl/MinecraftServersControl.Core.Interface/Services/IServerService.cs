using MinecraftServersControl.Core.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core.Interface.Services
{
    public interface IServerService
    {
        Task<Result<IEnumerable<ComputerDTO>>> GetServers();
        Task<Result<string>> GetOutput(TargetServerDTO serverState);
        Task<Result> Input(ServerInputDTO serverInput);
        Task<Result> Start(TargetServerDTO serverState);
        Task<Result> Terminate(TargetServerDTO serverState);

        event ResultEventHandler<TargetServerDTO> ServerStarted;
        event ResultEventHandler<TargetServerDTO> ServerStopped;
        event ResultEventHandler<ServerOutputDTO> ServerOutput;
    }
}
