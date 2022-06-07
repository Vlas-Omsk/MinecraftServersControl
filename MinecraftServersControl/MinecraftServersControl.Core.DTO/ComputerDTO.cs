using System;
using System.Collections.Generic;

namespace MinecraftServersControl.Core.DTO
{
    [Serializable]
    public sealed class ComputerDTO
    {
        public string Name { get; private set; }
        public IEnumerable<ServerDTO> Servers { get; private set; }

        private ComputerDTO()
        {
        }
        
        public ComputerDTO(string name, IEnumerable<ServerDTO> servers)
        {
            Name = name;
            Servers = servers;
        }
    }
}
