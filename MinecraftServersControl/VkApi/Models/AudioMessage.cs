using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class AudioMessage
    {
        [JsonProperty("duration")]
        public int Duration { get; set; }
        [JsonProperty("waveform")]
        public int[] Waveform { get; set; }
        [JsonProperty("link_ogg")]
        public string LinkOgg { get; set; }
        [JsonProperty("link_mp3")]
        public string LinkMp3 { get; set; }
    }
}
