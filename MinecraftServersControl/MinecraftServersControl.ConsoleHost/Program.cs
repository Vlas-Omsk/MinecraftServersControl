using MinecraftServersControl.API.WebSocket;
using System;

namespace MinecraftServersControl.ConsoleHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mainServer = new MainServer("ws://0.0.0.0:8888");

            mainServer.Start();

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
