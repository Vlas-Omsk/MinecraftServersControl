using MinecraftServersControl.Core.DTO;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core.Interface.Services
{
    public interface IUserService
    {
        Task<Result<SessionDTO>> SignIn(UserDTO userSchema);
        Task<Result<SessionDTO>> Restore(Guid sessionId);
        Task<Result> VerifySession(Guid sessionId);

        event ResultEventHandler<Guid> SessionRemoved;
    }
}
