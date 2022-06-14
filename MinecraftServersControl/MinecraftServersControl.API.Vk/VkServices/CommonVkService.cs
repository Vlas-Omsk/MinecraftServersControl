using MinecraftServersControl.Common;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Vk.VkServices
{
    public sealed class CommonVkService : VkService
    {
        [Command("команды")]
        [Description("Информация о всех командах")]
        public async Task Commands()
        {
            string result = "";

            foreach (var serviceType in Handler.ServiceTypes)
                foreach (var methodAttributePair in serviceType.GetMethodsWithAttribute<CommandAttribute>())
                    if (await Handler.CommandAccessVerifier.Verify(methodAttributePair.Method) == null)
                        result += FormatHelper.ToStringCommandShort(methodAttributePair.Method) + "\r\n";

            await Handler.MessageResponse.Send(result);
        }

        [Command("команда")]
        [Description("Информация о команде")]
        public async Task Command(
            [CommandParameter("строка")] params string[] path
        )
        {
            var result = ServiceHelper.GetCommandFromString(Handler.ServiceTypes, path);
            var resultMsg = "";

            if (result.Method != null)
            {
                resultMsg = FormatHelper.ToStringCommandLong(result.Method);
            }
            else if (result.Matches.Length == 1)
            {
                resultMsg = FormatHelper.ToStringCommandLong(result.Matches[0]);
            }
            else if (result.Matches.Length > 0)
            {
                foreach (var method in result.Matches)
                    resultMsg += FormatHelper.ToStringCommandShort(method) + "\r\n";
            }
            else
            {
                resultMsg = "Команда не найдена";
            }

            await Handler.MessageResponse.Send(resultMsg);
        }
    }
}
