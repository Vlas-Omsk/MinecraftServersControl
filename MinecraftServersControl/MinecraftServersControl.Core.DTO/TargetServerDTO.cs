using System;

namespace MinecraftServersControl.Core.DTO
{
    [Serializable]
    public sealed class TargetServerDTO
    {
        public Guid ComputerId { get; private set; }
        public Guid ServerId { get; private set; }

        private TargetServerDTO()
        {
        }

        public TargetServerDTO(Guid computerId, Guid serverId)
        {
            ComputerId = computerId;
            ServerId = serverId;
        }
    }
}
