using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftApi
{
    public class LegacyStreamWriter : IDisposable
    {
        private Stream _stream;
        private bool _leaveOpen;
        private static readonly Encoding _encoding = Encoding.GetEncoding("UTF-16BE");

        public LegacyStreamWriter(Stream stream, bool leaveOpen = false)
        {
            _stream = stream;
            _leaveOpen = leaveOpen;
        }

        public async Task WriteBytes(byte[] value)
        {
            await _stream.WriteAsync(value);
        }

        public async Task WriteBytes(IEnumerable<byte> value)
        {
            await WriteBytes(value.ToArray());
        }

        public async Task WriteByte(byte value)
        {
            await WriteBytes(new byte[] { value });
        }

        public async Task WriteInt(int value)
        {
            await WriteBytes(BitConverter.GetBytes(value).Reverse());
        }

        public async Task WriteShort(short value)
        {
            await WriteBytes(BitConverter.GetBytes(value).Reverse());
        }

        public async Task WriteString(string value)
        {
            await WriteShort((short)(value.Length));
            await WriteBytes(_encoding.GetBytes(value));
        }

        public void Dispose()
        {
            if (!_leaveOpen)
                _stream.Close();
        }
    }
}
