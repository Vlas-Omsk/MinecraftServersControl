using System;
using System.Collections.Generic;

namespace MinecraftServersControl.Core.DTO
{
    [Serializable]
    public sealed class ComputerDTO
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool Running { get; private set; }
        public IEnumerable<ServerDTO> Servers { get; private set; }

        private ComputerDTO()
        {
        }

        public ComputerDTO(Guid id, string name, bool running, IEnumerable<ServerDTO> servers)
        {
            Id = id;
            Name = name;
            Running = running;
            Servers = servers;
        }
    }
}
