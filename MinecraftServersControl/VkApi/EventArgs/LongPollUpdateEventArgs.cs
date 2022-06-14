using System;
using VkApi.Models;

namespace VkApi
{
    public sealed class LongPollUpdateEventArgs : EventArgs
    {
        public LongPollUpdate[] Updates { get; }

        public LongPollUpdateEventArgs(LongPollUpdate[] updates)
        {
            Updates = updates;
        }
    }
}
