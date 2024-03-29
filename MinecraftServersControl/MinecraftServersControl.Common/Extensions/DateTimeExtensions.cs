﻿using System;

namespace MinecraftServersControl.Common
{
    public static class DateTimeExtensions
    {
        public static double ToUnixTime(this DateTime dateTime)
        {
            return dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
