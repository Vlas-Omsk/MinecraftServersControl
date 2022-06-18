using System;

namespace MinecraftServersControl.Remote.Core.DTO
{
    public sealed class ServerInputDTO
    {
        public Guid ServerId { get; private set; }
        public string Message { get; private set; }

        private ServerInputDTO()
        {
        }

        public ServerInputDTO(Guid serverId, string message)
        {
            ServerId = serverId;
            Message = message;
        }
    }
}
