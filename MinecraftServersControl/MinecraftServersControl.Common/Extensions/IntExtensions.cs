﻿using System;

namespace MinecraftServersControl.Common
{
    public static class IntExtensions
    {
        public static DateTime ToDateTime(this int unixTime)
        {
            return new DateTime(1970, 1, 1).AddSeconds(unixTime);
        }
    }
}
