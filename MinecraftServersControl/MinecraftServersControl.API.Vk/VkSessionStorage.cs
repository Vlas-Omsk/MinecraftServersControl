using System;
using System.Collections.Generic;

namespace MinecraftServersControl.API.Vk
{
    public sealed class VkSessionStorage
    {
        private Dictionary<int, VkSession> _collection = new Dictionary<int, VkSession>();

        public VkSession GetOrCreate(int userId)
        {
            if (!_collection.TryGetValue(userId, out VkSession session))
                _collection.Add(userId, session = new VkSession());

            return session;
        }
    }
}
