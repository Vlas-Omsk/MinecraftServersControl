using System;

namespace MinecraftServersControl.Remote
{
    public sealed class ServerHost
    {
        public ServerInfo ServerInfo { get; }

        private ProcessHost _processHost = new ProcessHost();

        public ServerHost(ServerInfo serverInfo)
        {
            ServerInfo = serverInfo;

            _processHost.DataReceived += OnDataReceived; ;
            _processHost.Started += OnStarted;
            _processHost.Stopped += OnStopped;
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

        public void Input(string message)
        {
            _processHost.Write(message);
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            DataReceived?.Invoke(this, e);
        }

        private void OnStarted(object sender, EventArgs e)
        {
            Started?.Invoke(this, e);
        }

        private void OnStopped(object sender, EventArgs e)
        {
            Stopped?.Invoke(this, e);
        }

        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public event EventHandler Started;
        public event EventHandler Stopped;
    }
}
