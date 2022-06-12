using MinecraftServersControl.Core.DTO;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core.Interface.Services
{
    public interface IComputerService
    {
        Task<Result> VerifyComputer(Guid computerKey);

        event ResultEventHandler<ComputerStateDTO> ComputerStarted;
        event ResultEventHandler<ComputerStateDTO> ComputerStopped;
    }
}
