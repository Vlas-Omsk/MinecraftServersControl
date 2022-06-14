using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Vk.VkServices
{
    [VkService("пользователь")]
    public sealed class UserVkService : VkService
    {
        [VkCommand("войти")]
        public async Task SignIn(
            [VkCommandParameter("логин")] string login,
            [VkCommandParameter("пароль")] string password
        )
        {
            await SendResultCode(await Application.VkUserService.SignIn(Message.FromId, login, password));
        }

        [VkCommand("инфо")]
        public async Task Info()
        {
            var result = await Application.VkUserService.GetUserInfo(Message.FromId);

            if (result.HasErrors())
            {
                await SendResultCode(result);
                return;
            }

            await Send($"Логин: {result.Data.Login}");
        }
    }
}
