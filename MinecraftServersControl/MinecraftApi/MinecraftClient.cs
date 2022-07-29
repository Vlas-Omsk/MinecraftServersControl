using System;
using System.Net;
using System.Net.Sockets;

namespace MinecraftApi
{
    public sealed class MinecraftClient
    {
        private Socket _socket;

        public void Connect(string host, int port)
        {
            var endPoint = new DnsEndPoint(host, port, AddressFamily.InterNetwork);
            _socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(endPoint);
        }
    }
}
