using System;

namespace MinecraftServersControl.Core.Models
{
    public sealed class ComputerServerAliasesPair
    {
        public string ComputerAlias { get; }
        public string ServerAlias { get; }

        public ComputerServerAliasesPair(string computerAlias, string serverAlias)
        {
            ComputerAlias = computerAlias;
            ServerAlias = serverAlias;
        }
    }
}
