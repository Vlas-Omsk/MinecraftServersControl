using System;

namespace MinecraftServersControl.Core.DTO
{
    [Serializable]
    public sealed class ServerDTO
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool Running { get; private set; }

        private ServerDTO()
        {
        }

        public ServerDTO(Guid id, string name, bool running)
        {
            Id = id;
            Name = name;
            Running = running;
        }
    }
}
