using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class MessageActionPhoto
    {
        [JsonProperty("photo_50")]
        public string Photo50 { get; private set; }
        [JsonProperty("photo_100")]
        public string Photo100 { get; private set; }
        [JsonProperty("photo_200")]
        public string Photo200 { get; private set; }

        private MessageActionPhoto()
        {
        }
    }
}
