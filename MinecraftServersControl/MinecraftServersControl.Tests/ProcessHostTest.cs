using MinecraftServersControl.Remote.Core;
using MinecraftServersControl.Remote.Core.IO;
using System;

namespace MinecraftServersControl.Tests
{
    internal sealed class ProcessHostTest
    {
        public void Start()
        {
            var processHost = new ProcessHost();
            processHost.DataReceived += OnProcessHostDataReceived;
            processHost.Start("cmd");

            while (true)
            {
                var line = Console.ReadLine();
                processHost.Write(line);
            }
        }

        private void OnProcessHostDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.Write(e.Data);
        }
    }
}
