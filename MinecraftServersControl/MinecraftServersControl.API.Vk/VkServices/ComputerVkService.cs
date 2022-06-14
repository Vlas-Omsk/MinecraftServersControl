using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Vk.VkServices
{
    [VkService("пк")]
    public sealed class ComputerVkService : VkService
    {
        [VkCommand("инфо")]
        public async Task Info()
        {
            var result = await Application.VkUserService.CheckAccess(Message.FromId);

            if (result.HasErrors())
            {
                await SendResultCode(result);
                return;
            }

            var result2 = await Application.ServerService.GetServers();

            if (result.HasErrors())
            {
                await SendResultCode(result);
                return;
            }

            var str = string.Empty;

            foreach (var computer in result2.Data)
            {
                str += $"Компьютер {computer.Alias} ({computer.Name}): {(computer.Running ? "включен" : "отключен")}\r\n" +
                    $"Сервера:\r\n";

                foreach (var server in computer.Servers)
                    str += $"{server.Alias} ({server.Name}): {(server.Running ? "включен" : "отключен")}\r\n";

                str += "\r\n";
            }

            await Send(str);
        }
    }
}
