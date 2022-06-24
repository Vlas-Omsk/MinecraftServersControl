using MinecraftServersControl.Core.DTO;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core.Abstractions.Services
{
    public interface IServerService
    {
        Task<ComputerDTO[]> GetServers();
        Task<string> GetOutput(TargetServerDTO serverState);
        Task Input(ServerInputDTO serverInput);
        Task Start(TargetServerDTO serverState);
        Task Terminate(TargetServerDTO serverState);

        event EventHandler<TargetServerDTO> ServerStarted;
        event EventHandler<TargetServerDTO> ServerStopped;
        event EventHandler<ServerOutputDTO> ServerOutput;
    }
}
