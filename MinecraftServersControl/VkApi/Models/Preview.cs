using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Preview
    {
        [JsonProperty("photo")]
        public Photo Photo { get; set; }
        [JsonProperty("graffiti")]
        public Graffiti Graffiti { get; set; }
        [JsonProperty("audio_message")]
        public AudioMessage AudioMessage { get; set; }
    }
}
