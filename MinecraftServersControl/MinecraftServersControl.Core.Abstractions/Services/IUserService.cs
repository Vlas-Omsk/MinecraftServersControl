using MinecraftServersControl.Core.DTO;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core.Abstractions.Services
{
    public interface IUserService
    {
        Task<SessionDTO> SignIn(UserDTO userSchema);
        Task<SessionDTO> Restore(Guid sessionId);
        Task VerifySession(Guid sessionId);

        event EventHandler<Guid> SessionRemoved;
    }
}
