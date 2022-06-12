using System;

namespace MinecraftServersControl.Core.DTO
{
    public sealed class ServerOutputDTO
    {
        public Guid ComputerId { get; private set; }
        public Guid ServerId { get; private set; }
        public string Output { get; private set; }

        private ServerOutputDTO()
        {
        }

        public ServerOutputDTO(Guid computerId, Guid serverId, string output)
        {
            ComputerId = computerId;
            ServerId = serverId;
            Output = output;
        }
    }
}
