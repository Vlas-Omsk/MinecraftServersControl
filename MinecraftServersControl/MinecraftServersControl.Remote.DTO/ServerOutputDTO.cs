using System;

namespace MinecraftServersControl.Remote.DTO
{
    public sealed class ServerOutputDTO
    {
        public Guid ServerId { get; private set; }
        public string Output { get; private set; }

        private ServerOutputDTO()
        {
        }

        public ServerOutputDTO(Guid serverKey, string output)
        {
            ServerId = serverKey;
            Output = output;
        }
    }
}
