using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Audio
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }
        [JsonProperty("artist")]
        public string Artist { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("duration")]
        public int Duration { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("lyrics_id")]
        public int LyricsId { get; set; }
        [JsonProperty("album_id")]
        public int AlbumId { get; set; }
        [JsonProperty("genre_id")]
        public int GenreId { get; set; }
        [JsonProperty("date")]
        public int Date { get; set; }
        [JsonProperty("no_search")]
        public bool NoSearch { get; set; }
        [JsonProperty("is_hq")]
        public bool IsHQ { get; set; }
    }
}
