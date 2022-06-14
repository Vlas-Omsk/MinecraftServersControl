using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MinecraftServersControl.Common
{
    public static class StringExtensions
    {
        public static byte[] ToByteArray(this string hexString)
        {
            var bytes = new byte[hexString.Length / 2];

            try
            {
                for (int i = 0, j = 0; i < hexString.Length; i += 2, j++)
                    bytes[j] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            catch (Exception ex)
            {
                throw new Exception("Incorrect hexadecimal byte string format", ex);
            }

            return bytes;
        }

        public static string ToSha256Hash(this string password)
        {
            return string.Join("", password.ToSha256HashBytes().Select(x => x.ToString("X2")));
        }

        public static byte[] ToSha256HashBytes(this string password)
        {
            return SHA256.HashData(Encoding.UTF8.GetBytes(password));
        }
    }
}
