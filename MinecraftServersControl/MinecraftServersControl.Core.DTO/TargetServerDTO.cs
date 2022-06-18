using System;

namespace MinecraftServersControl.Core.DTO
{
    public sealed class TargetServerDTO
    {
        public string ComputerAlias { get; private set; }
        public string ServerAlias { get; private set; }

        private TargetServerDTO()
        {
        }

        public TargetServerDTO(string computerAlias, string serverAlias)
        {
            ComputerAlias = computerAlias;
            ServerAlias = serverAlias;
        }
    }
}
