using System;
using System.Collections.Generic;

namespace MinecraftServersControl.Core.DTO
{
    public sealed class ComputerDTO
    {
        public string Name { get; private set; }
        public string Alias { get; private set; }
        public bool Running { get; private set; }
        public IEnumerable<ServerDTO> Servers { get; private set; }

        private ComputerDTO()
        {
        }

        public ComputerDTO(string name, string alias, bool running, IEnumerable<ServerDTO> servers)
        {
            Name = name;
            Alias = alias;
            Running = running;
            Servers = servers;
        }
    }
}
