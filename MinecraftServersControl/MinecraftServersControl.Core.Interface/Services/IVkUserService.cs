using MinecraftServersControl.Core.DTO;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core.Interface.Services
{
    public interface IVkUserService
    {
        Task<Result> SignIn(int vkUserId, string login, string password);
        Task<Result<UserInfoDTO>> GetUserInfo(int vkUserId);
        Task<Result> CheckAccess(int vkUserId);
    }
}
