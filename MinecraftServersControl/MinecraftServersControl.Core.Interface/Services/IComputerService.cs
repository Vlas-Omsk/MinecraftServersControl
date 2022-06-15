using MinecraftServersControl.Core.DTO;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core.Interface.Services
{
    public interface IComputerService
    {
        Task<bool> VerifyComputer(Guid computerKey);

        event EventHandler<ComputerStateDTO> ComputerStarted;
        event EventHandler<ComputerStateDTO> ComputerStopped;
    }
}
