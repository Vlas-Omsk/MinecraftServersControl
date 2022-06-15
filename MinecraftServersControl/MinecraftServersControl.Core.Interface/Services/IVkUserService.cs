using MinecraftServersControl.Core.DTO;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core.Interface.Services
{
    public interface IVkUserService
    {
        Task SignIn(int vkUserId, string login, string password);
        Task SignOut(int vkUserId);
        Task<UserInfoDTO> GetUserInfo(int vkUserId);
        Task<bool> IsAuthorized(int vkUserId);
    }
}
