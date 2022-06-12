using System;

namespace MinecraftServersControl.Remote.Server
{
    public sealed class ServerStateChangedEventArgs : EventArgs
    {
        public Guid ComputerId { get; }
        public Guid ServerId { get; }

        public ServerStateChangedEventArgs(Guid computerId, Guid serverId)
        {
            ComputerId = computerId;
            ServerId = serverId;
        }
    }
}
