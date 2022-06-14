using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Vk.VkServices
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
            await Handler.MessageResponse.SendResultCode((await Handler.Application.VkUserService.SignIn(Handler.Message.FromId, login, password)).Code);
        }

        [Command("выйти")]
        [AuthorizedOnly]
        public async Task SignOut()
        {
            await Handler.MessageResponse.SendResultCode((await Handler.Application.VkUserService.SignOut(Handler.Message.FromId)).Code);
        }

        [Command("инфо")]
        [AuthorizedOnly]
        public async Task Info()
        {
            var result = await Handler.Application.VkUserService.GetUserInfo(Handler.Message.FromId);

            if (result.HasErrors())
            {
                await Handler.MessageResponse.SendResultCode(result.Code);
                return;
            }

            await Handler.MessageResponse.Send($"Логин: {result.Data.Login}");
        }
    }
}
