using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MinecraftServersControl.API.IntegrationTests
{
    public static class StringExtensions
    {
        public static string ToSha256Hash(this string password)
        {
            return string.Join("", SHA256.HashData(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
        }
    }
}
