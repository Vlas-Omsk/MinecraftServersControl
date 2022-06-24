using System;

namespace MinecraftServersControl.Remote.Core
{
    public sealed class ServerInfo
    {
        public Guid Id { get; private set; }
        public string Path { get; private set; }
    }
}
