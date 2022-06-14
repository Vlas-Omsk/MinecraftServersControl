using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Photo
    {
        [JsonProperty("id")]
        public int Id { get; private set; }
        [JsonProperty("album_id")]
        public int AlbumId { get; private set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; private set; }
        [JsonProperty("user_id")]
        public int UserId { get; private set; }
        [JsonProperty("text")]
        public string Text { get; private set; }
        [JsonProperty("date")]
        public int Date { get; private set; }
        [JsonProperty("sizes")]
        public VkSize[] Sizes { get; private set; }
        [JsonProperty("width")]
        public int Width { get; private set; }
        [JsonProperty("height")]
        public int Height { get; private set; }

        private Photo()
        {
        }
    }
}
