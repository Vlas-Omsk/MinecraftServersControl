﻿using System;

namespace MinecraftServersControl.Core
{
    internal static class StringExtensions
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
    }
}
