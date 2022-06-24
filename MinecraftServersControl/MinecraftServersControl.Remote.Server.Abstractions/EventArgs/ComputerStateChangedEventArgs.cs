using System;

namespace MinecraftServersControl.Remote.Server.Abstractions
{
    public sealed class ComputerStateChangedEventArgs : EventArgs
    {
        public Guid ComputerId { get; }

        public ComputerStateChangedEventArgs(Guid computerId)
        {
            ComputerId = computerId;
        }
    }
}
