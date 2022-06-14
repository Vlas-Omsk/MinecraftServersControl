using System;

namespace MinecraftServersControl.Core.DTO
{
    public sealed class ServerOutputDTO
    {
        public string ComputerAlias { get; private set; }
        public string ServerAlias { get; private set; }
        public string Output { get; private set; }

        private ServerOutputDTO()
        {
        }

        public ServerOutputDTO(string computerAlias, string serverAlias, string output)
        {
            ComputerAlias = computerAlias;
            ServerAlias = serverAlias;
            Output = output;
        }
    }
}
