using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class AudioMessage
    {
        [JsonProperty("duration")]
        public int Duration { get; private set; }
        [JsonProperty("waveform")]
        public int[] Waveform { get; private set; }
        [JsonProperty("link_ogg")]
        public string LinkOgg { get; private set; }
        [JsonProperty("link_mp3")]
        public string LinkMp3 { get; private set; }

        private AudioMessage()
        {
        }
    }
}
