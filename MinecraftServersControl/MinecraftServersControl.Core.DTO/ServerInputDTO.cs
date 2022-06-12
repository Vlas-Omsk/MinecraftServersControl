using System;

namespace MinecraftServersControl.Core.DTO
{
    public sealed class ServerInputDTO
    {
        public Guid ComputerId { get; private set; }
        public Guid ServerId { get; private set; }
        public string Message { get; private set; }

        private ServerInputDTO()
        {
        }

        public ServerInputDTO(Guid computerId, Guid serverId, string message)
        {
            ComputerId = computerId;
            ServerId = serverId;
            Message = message;
        }
    }
}
