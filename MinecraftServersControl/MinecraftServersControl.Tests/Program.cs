using MinecraftApi;
using System;
using System.IO;

namespace MinecraftServersControl.Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Start();

            while (true)
            {
                Console.ReadLine();
            }
        }

        async static void Start()
        {
            var legacyProtocol = new Protocol78();

            legacyProtocol.Connect();

            await legacyProtocol.ServerListPing();

            //using (var memoryStream = new MemoryStream())
            //using (var streamWriter = new LegacyStreamWriter(memoryStream))
            //{
            //    await streamWriter.WriteByte(0x49);
            //    await streamWriter.WriteString("localhost");
            //    await streamWriter.WriteInt(25565);

            //    await legacyProtocol.PluginMessage("MC|PingHost", memoryStream.ToArray());
            //}

            var bytes = await legacyProtocol.Get(1000);
        }
    }
}
