using System;

namespace MinecraftServersControl.Remote.Core.DTO
{
    public sealed class ServerInfoDTO
    {
        public Guid Id { get; private set; }
        public bool Running { get; private set; }

        private ServerInfoDTO()
        {
        }

        public ServerInfoDTO(Guid key, bool running)
        {
            Id = key;
            Running = running;
        }
    }
}
