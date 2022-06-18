using System;

namespace MinecraftServersControl.Remote.Server.Abstractions
{
    public sealed class ServerOutputEventArgs : EventArgs
    {
        public Guid ComputerId { get; }
        public Guid ServerId { get; }
        public string Output { get; }

        public ServerOutputEventArgs(Guid computerId, Guid serverId, string output)
        {
            ComputerId = computerId;
            ServerId = serverId;
            Output = output;
        }
    }
}
