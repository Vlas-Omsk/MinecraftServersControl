using System;

namespace MinecraftServersControl.Core.DTO
{
    [Serializable]
    public sealed class ServerDTO
    {
        public string Alias { get; private set; }
        public string Name { get; private set; }
        public bool Running { get; private set; }

        private ServerDTO()
        {
        }

        public ServerDTO(string alias, string name, bool running)
        {
            Alias = alias;
            Name = name;
            Running = running;
        }
    }
}
