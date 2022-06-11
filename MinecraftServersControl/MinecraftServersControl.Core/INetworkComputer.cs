using MinecraftServersControl.Remote.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core
{
    public interface INetworkComputer
    {
        Task<Result<IEnumerable<ServerDTO>>> GetInfo(Guid computerKey);
        Task<Result<string>> GetOutput(Guid computerKey, Guid serverKey);
        Task<Result> Start(Guid computerKey, Guid serverKey);
        Task<Result> Terminate(Guid computerKey, Guid serverKey);
    }
}
