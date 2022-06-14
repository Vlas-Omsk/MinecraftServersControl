using MinecraftServersControl.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Vk.VkServices
{
    public sealed class CommonVkService : VkService
    {
        [VkCommand("команды")]
        public async Task Commands()
        {
            string result = "";

            foreach (var serviceType in ServiceTypes)
                foreach (var methodAttributePair in serviceType.GetMethodsWithAttribute<VkCommandAttribute>())
                    result += ServiceHelper.FormatCommandShort(methodAttributePair.Method) + "\r\n";

            await Send(result);
        }

        [VkCommand("команда")]
        public async Task Command([VkCommandParameter("строка")] params string[] path)
        {
            var result = ServiceHelper.GetCommandFromString(ServiceTypes, path);
            var resultMsg = "";

            if (result.Method != null)
            {
                resultMsg = ServiceHelper.FormatCommandLong(result.Method);
            }
            else if (result.Matches.Any())
            {
                foreach (var method in result.Matches)
                    resultMsg += ServiceHelper.FormatCommandShort(method) + "\r\n";
            }
            else
            {
                resultMsg = "Команда не найдена";
            }

            await Send(resultMsg);
        }
    }
}
