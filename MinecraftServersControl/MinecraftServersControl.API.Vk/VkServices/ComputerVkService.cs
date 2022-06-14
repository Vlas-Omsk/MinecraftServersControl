using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Vk.VkServices
{
    [Service("пк")]
    public sealed class ComputerVkService : VkService
    {
        [Command("инфо")]
        [AuthorizedOnly]
        public async Task Info()
        {
            var result = await Handler.Application.ServerService.GetServers();

            if (result.HasErrors())
            {
                await Handler.MessageResponse.SendResultCode(result.Code);
                return;
            }

            var str = string.Empty;

            foreach (var computer in result.Data)
            {
                str += $"Компьютер {computer.Alias} ({computer.Name}): {FormatHelper.ToStringOnOff(computer.Running)}\r\n" +
                    $"Сервера:\r\n";

                foreach (var server in computer.Servers)
                    str += $"{server.Alias} ({server.Name}): {FormatHelper.ToStringOnOff(server.Running)}\r\n";

                str += "\r\n";
            }

            await Handler.MessageResponse.Send(str);
        }
    }
}
