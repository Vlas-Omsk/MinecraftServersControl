using System;

namespace MinecraftServersControl.Core.DTO
{
    [Serializable]
    public sealed class ServerDTO
    {
        public string Name { get; private set; }

        private ServerDTO()
        {
        }

        public ServerDTO(string name)
        {
            Name = name;
        }
    }
}
