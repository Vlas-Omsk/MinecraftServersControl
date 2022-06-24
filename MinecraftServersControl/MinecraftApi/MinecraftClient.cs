using System;
using System.Net;
using System.Net.Sockets;

namespace MinecraftApi
{
    public sealed class MinecraftClient
    {
        private Socket _socket;

        public void Connect()
        {
            var endPoint = new DnsEndPoint("192.168.1.100", 25565, AddressFamily.InterNetwork);
            _socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(endPoint);
        }
    }
}
