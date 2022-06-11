using System;

namespace MinecraftServersControl.Remote.DTO
{
    public sealed class ServerDTO
    {
        public Guid Id { get; private set; }
        public bool Running { get; private set; }

        private ServerDTO()
        {
        }

        public ServerDTO(Guid key, bool running)
        {
            Id = key;
            Running = running;
        }
    }
}
