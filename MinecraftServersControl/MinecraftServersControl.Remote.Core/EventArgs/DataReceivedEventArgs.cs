using System;

namespace MinecraftServersControl.Remote.Core
{
    public sealed class DataReceivedEventArgs : EventArgs
    {
        public string Data { get; }

        public DataReceivedEventArgs(string data)
        {
            Data = data;
        }
    }
}
