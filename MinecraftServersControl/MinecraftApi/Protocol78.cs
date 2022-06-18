using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MinecraftApi
{
    public sealed class Protocol78
    {
        private Socket _socket;

        public async Task ServerListPing()
        {
            await SendAsync(new byte[] { 0xFE, 0x01 });
        }

        public async Task PluginMessage(string channel, byte[] data)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new LegacyStreamWriter(memoryStream);

            await streamWriter.WriteByte(0xFA);
            await streamWriter.WriteString(channel);
            await streamWriter.WriteShort((short)data.Length);
            await streamWriter.WriteBytes(data);

            await SendAsync(memoryStream.ToArray());
        }

        private async Task SendAsync(byte[] value)
        {
            Console.WriteLine(string.Concat(value.Select(x => x.ToString("x2"))));

            await _socket.SendAsync(value, SocketFlags.None);
        }

        public async Task<byte[]> Get(int length)
        {
            var bytes = new byte[length];
            await _socket.ReceiveAsync(bytes, SocketFlags.None);
            return bytes;
        }
    }
}
