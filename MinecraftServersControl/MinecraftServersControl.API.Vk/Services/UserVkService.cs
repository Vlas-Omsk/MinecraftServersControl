using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Vk.Services
{
    [Service("пользователь")]
    public sealed class UserVkService : VkService
    {
        [Command("войти")]
        public async Task SignIn(
            [CommandParameter("логин")] string login,
            [CommandParameter("пароль")] string password
        )
        {
            await Handler.Application.VkUserService.SignIn(Handler.Message.FromId, login, password);
            await Handler.MessageResponse.SendSuccess();
        }

        [Command("выйти")]
        [AuthorizedOnly]
        public async Task SignOut()
        {
            await Handler.Application.VkUserService.SignOut(Handler.Message.FromId);
            await Handler.MessageResponse.SendSuccess();
        }

        [Command("инфо")]
        [AuthorizedOnly]
        public async Task Info()
        {
            var userInfo = await Handler.Application.VkUserService.GetUserInfo(Handler.Message.FromId);
            await Handler.MessageResponse.Send($"Логин: {userInfo.Login}");
        }
    }
}
