using System;

namespace MinecraftServersControl.Remote
{
    public sealed class ServerInfo
    {
        public Guid Id { get; private set; }
        public string Path { get; private set; }
    }
}
