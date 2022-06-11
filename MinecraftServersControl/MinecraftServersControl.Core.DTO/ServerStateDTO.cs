using System;

namespace MinecraftServersControl.Core.DTO
{
    [Serializable]
    public sealed class ServerStateDTO
    {
        public Guid ComputerKey { get; private set; }
        public Guid ServerKey { get; private set; }

        private ServerStateDTO()
        {
        }

        public ServerStateDTO(Guid computerKey, Guid serverKey)
        {
            ComputerKey = computerKey;
            ServerKey = serverKey;
        }
    }
}
