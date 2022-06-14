using System;

namespace MinecraftServersControl.Core.DTO
{
    public sealed class ServerInputDTO
    {
        public string ComputerAlias { get; private set; }
        public string ServerAlias { get; private set; }
        public string Message { get; private set; }

        private ServerInputDTO()
        {
        }

        public ServerInputDTO(string computerAlias, string serverAlias, string message)
        {
            ComputerAlias = computerAlias;
            ServerAlias = serverAlias;
            Message = message;
        }
    }
}
