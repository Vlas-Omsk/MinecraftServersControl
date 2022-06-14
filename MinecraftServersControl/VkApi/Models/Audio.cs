using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Audio
    {
        [JsonProperty("id")]
        public int Id { get; private set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; private set; }
        [JsonProperty("artist")]
        public string Artist { get; private set; }
        [JsonProperty("title")]
        public string Title { get; private set; }
        [JsonProperty("duration")]
        public int Duration { get; private set; }
        [JsonProperty("url")]
        public string Url { get; private set; }
        [JsonProperty("lyrics_id")]
        public int LyricsId { get; private set; }
        [JsonProperty("album_id")]
        public int AlbumId { get; private set; }
        [JsonProperty("genre_id")]
        public int GenreId { get; private set; }
        [JsonProperty("date")]
        public int Date { get; private set; }
        [JsonProperty("no_search")]
        public bool NoSearch { get; private set; }
        [JsonProperty("is_hq")]
        public bool IsHQ { get; private set; }

        private Audio()
        {
        }
    }
}
