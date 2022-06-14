using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Preview
    {
        [JsonProperty("photo")]
        public Photo Photo { get; private set; }
        [JsonProperty("graffiti")]
        public Graffiti Graffiti { get; private set; }
        [JsonProperty("audio_message")]
        public AudioMessage AudioMessage { get; private set; }

        private Preview()
        {
        }
    }
}
