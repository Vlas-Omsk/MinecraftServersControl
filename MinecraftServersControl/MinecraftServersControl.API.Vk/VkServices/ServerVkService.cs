using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Core.Interface;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Vk.VkServices
{
    [VkService("сервер")]
    public sealed class ServerVkService : VkService
    {
        [VkCommand("запустить")]
        public async Task Start(
            [VkCommandParameter("компьютер")] string computerAlias,
            [VkCommandParameter("сервер")] string serverAlias
        )
        {
            var result = await Application.VkUserService.CheckAccess(Message.FromId);

            if (result.HasErrors())
            {
                await SendResultCode(result);
                return;
            }

            var result2 = await Application.ServerService.Start(new TargetServerDTO(computerAlias, serverAlias));

            await SendResultCode(result2);
        }

        [VkCommand("остановить")]
        public async Task Terminate(
            [VkCommandParameter("компьютер")] string computerAlias,
            [VkCommandParameter("сервер")] string serverAlias
        )
        {
            var result = await Application.VkUserService.CheckAccess(Message.FromId);

            if (result.HasErrors())
            {
                await SendResultCode(result);
                return;
            }

            var result2 = await Application.ServerService.Terminate(new TargetServerDTO(computerAlias, serverAlias));

            await SendResultCode(result2);
        }

        [VkCommand("команда")]
        public async Task Command(
            [VkCommandParameter("компьютер")] string computerAlias,
            [VkCommandParameter("сервер")] string serverAlias,
            [VkCommandParameter("команда")] params string[] command
        )
        {
            var result = await Application.VkUserService.CheckAccess(Message.FromId);

            if (result.HasErrors())
            {
                await SendResultCode(result);
                return;
            }

            var result2 = await Application.ServerService.Input(new ServerInputDTO(computerAlias, serverAlias, string.Join(' ', command)));

            await SendResultCode(result2);
        }

        [VkCommand("лог")]
        public async Task Log(
            [VkCommandParameter("компьютер")] string computerAlias,
            [VkCommandParameter("сервер")] string serverAlias
        )
        {
            var result = await Application.VkUserService.CheckAccess(Message.FromId);

            if (result.HasErrors())
            {
                await SendResultCode(result);
                return;
            }

            var result2 = await Application.ServerService.GetOutput(new TargetServerDTO(computerAlias, serverAlias));

            await Send(result2.Data);
        }

        [VkCommand("консоль")]
        public async Task Console(
            [VkCommandParameter("компьютер")] string computerAlias,
            [VkCommandParameter("сервер")] string serverAlias
        )
        {
            if (Message.IsFromChat())
            {
                await SendMethodUnavailableFromChat();
                return;
            }

            var result = await Application.VkUserService.CheckAccess(Message.FromId);

            if (result.HasErrors())
            {
                await SendResultCode(result);
                return;
            }

            var result2 = await Application.ServerService.GetOutput(new TargetServerDTO(computerAlias, serverAlias));

            await Send("Для выходя используйте '&выход'\r\n\r\n" + result2.Data);

            ResultEventHandler<ServerOutputDTO> handler = async (sender, result) =>
            {
                if (result.Data.ComputerAlias != computerAlias &&
                    result.Data.ServerAlias != serverAlias)
                    return;

                await Send(result.Data.Output);
            };

            Application.ServerService.ServerOutput += handler;

            Session.HandlerOverride = async (message) =>
            {
                if (message.Text.Equals("&выход"))
                {
                    Application.ServerService.ServerOutput -= handler;
                    Session.HandlerOverride = null;
                    await Send("Успешно");
                    return;
                }

                await Application.ServerService.Input(new ServerInputDTO(computerAlias, serverAlias, message.Text));
            };
        }
    }
}
