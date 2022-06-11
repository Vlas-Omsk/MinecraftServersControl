using System;

namespace MinecraftServersControl.Core
{
    public interface INetworkServer
    {
        INetworkComputer GetComputer(Guid computerKey);
    }
}
