using MinecraftServersControl.Core.DTO;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Vk.Services
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
            await Handler.MessageResponse.Send(
                await Handler.Application.ServerService.GetOutput(new TargetServerDTO(computerAlias, serverAlias))
            );
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

            EventHandler<ServerOutputDTO> serverOutputHandler = null;
            EventHandler<TargetServerDTO> serverStoppedHandler = null;

            serverOutputHandler = async (sender, e) =>
            {
                if (e.ComputerAlias != computerAlias &&
                    e.ServerAlias != serverAlias)
                    return;

                await Handler.MessageResponse.Send(e.Output);
            };

            serverStoppedHandler = async (sender, e) =>
            {
                if (e.ComputerAlias != computerAlias &&
                    e.ServerAlias != serverAlias)
                    return;

                Handler.Application.ServerService.ServerOutput -= serverOutputHandler;
                Handler.Application.ServerService.ServerStopped -= serverStoppedHandler;
                Handler.Session.HandlerOverride = null;

                await Handler.MessageResponse.Send("Сервер остановлен");
            };

            Handler.Application.ServerService.ServerOutput += serverOutputHandler;
            Handler.Application.ServerService.ServerStopped += serverStoppedHandler;

            Handler.Session.HandlerOverride = async (message) =>
            {
                if (message.Text.Equals("&выход", StringComparison.OrdinalIgnoreCase))
                {
                    Handler.Application.ServerService.ServerOutput -= serverOutputHandler;
                    Handler.Application.ServerService.ServerStopped -= serverStoppedHandler;
                    Handler.Session.HandlerOverride = null;

                    await Handler.MessageResponse.Send("Успешно");
                }
                else
                {
                    await Handler.Application.ServerService.Input(new ServerInputDTO(computerAlias, serverAlias, message.Text));
                }
            };
        }
    }
}
