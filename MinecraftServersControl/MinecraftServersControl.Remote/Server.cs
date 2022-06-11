using System;

namespace MinecraftServersControl.Remote
{
    public sealed class Server
    {
        public ServerInfo ServerInfo { get; }

        private ProcessHost _processHost = new ProcessHost();

        public Server(ServerInfo serverInfo)
        {
            ServerInfo = serverInfo;

            _processHost.DataReceived += DataReceived;
            _processHost.Started += Started;
            _processHost.Stopped += Stopped;
        }

        public bool Running => _processHost.Running;
        public string Buffer => string.Concat(_processHost.Buffer);

        public void Start()
        {
            _processHost.Start(ServerInfo.Path);
        }

        public void Stop()
        {
            _processHost.Stop();
        }

        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public event EventHandler Started;
        public event EventHandler Stopped;
    }
}
