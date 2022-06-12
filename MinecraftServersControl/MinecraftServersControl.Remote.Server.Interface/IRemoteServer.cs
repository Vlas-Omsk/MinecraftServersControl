using MinecraftServersControl.Remote.Server;
using MinecraftServersControl.Remote.Server.Interface;
using System;

namespace MinecraftServersControl.Core
{
    public interface IRemoteServer
    {
        IRemoteComputer GetComputer(Guid computerKey);

        event EventHandler<ComputerStateChangedEventArgs> ComputerStarted;
        event EventHandler<ComputerStateChangedEventArgs> ComputerStopped;
        event EventHandler<ServerStateChangedEventArgs> ServerStarted;
        event EventHandler<ServerStateChangedEventArgs> ServerStopped;
        event EventHandler<ServerOutputEventArgs> ServerOutput;
    }
}
