using System;

namespace MinecraftServersControl.Core.Models
{
    internal sealed class ComputerServerIdPair
    {
        public IRemoteComputer Computer { get; }
        public Guid ServerId { get; }

        public ComputerServerIdPair(IRemoteComputer computer, Guid serverId)
        {
            Computer = computer;
            ServerId = serverId;
        }
    }
}
