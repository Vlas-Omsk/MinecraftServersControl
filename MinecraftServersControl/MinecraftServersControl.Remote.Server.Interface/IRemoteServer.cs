using System;

namespace MinecraftServersControl.Remote.Server.Abstractions
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
