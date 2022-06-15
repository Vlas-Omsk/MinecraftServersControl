using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Core.Interface;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Vk.VkServices
{
    [Service("сервер")]
    public sealed class ServerVkService : VkService
    {
        [Command("запустить")]
        [AuthorizedOnly]
        public async Task Start(
            [CommandParameter("компьютер")] string computerAlias,
            [CommandParameter("сервер")] string serverAlias
        )
        {
            await Handler.Application.ServerService.Start(new TargetServerDTO(computerAlias, serverAlias));
            await Handler.MessageResponse.SendSuccess();
        }

        [Command("остановить")]
        [AuthorizedOnly]
        public async Task Terminate(
            [CommandParameter("компьютер")] string computerAlias,
            [CommandParameter("сервер")] string serverAlias
        )
        {
            await Handler.Application.ServerService.Terminate(new TargetServerDTO(computerAlias, serverAlias));
            await Handler.MessageResponse.SendSuccess();
        }

        [Command("команда")]
        [AuthorizedOnly]
        public async Task Command(
            [CommandParameter("компьютер")] string computerAlias,
            [CommandParameter("сервер")] string serverAlias,
            [CommandParameter("команда")] params string[] command
        )
        {
            await Handler.Application.ServerService.Input(new ServerInputDTO(computerAlias, serverAlias, string.Join(' ', command)));
            await Handler.MessageResponse.SendSuccess();
        }

        [Command("лог")]
        [AuthorizedOnly]
        public async Task Log(
            [CommandParameter("компьютер")] string computerAlias,
            [CommandParameter("сервер")] string serverAlias
        )
        {
            await Handler.Application.ServerService.GetOutput(new TargetServerDTO(computerAlias, serverAlias));
            await Handler.MessageResponse.SendSuccess();
        }

        [Command("консоль")]
        [AuthorizedOnly]
        [NonChat]
        public async Task Console(
            [CommandParameter("компьютер")] string computerAlias,
            [CommandParameter("сервер")] string serverAlias
        )
        {
            var output = await Handler.Application.ServerService.GetOutput(new TargetServerDTO(computerAlias, serverAlias));

            await Handler.MessageResponse.Send("Для выходя используйте '&выход'\r\n\r\n" + output);

            EventHandler<ServerOutputDTO> handler = async (sender, e) =>
            {
                if (e.ComputerAlias != computerAlias &&
                    e.ServerAlias != serverAlias)
                    return;

                await Handler.MessageResponse.Send(e.Output);
            };

            Handler.Application.ServerService.ServerOutput += handler;

            Handler.Session.HandlerOverride = async (message) =>
            {
                if (message.Text.Equals("&выход"))
                {
                    Handler.Application.ServerService.ServerOutput -= handler;
                    Handler.Session.HandlerOverride = null;
                    await Handler.MessageResponse.Send("Успешно");
                    return;
                }

                await Handler.Application.ServerService.Input(new ServerInputDTO(computerAlias, serverAlias, message.Text));
            };
        }
    }
}
