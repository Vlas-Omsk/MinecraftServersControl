using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Donut
    {
        [JsonProperty("is_donut")]
        public bool IsDonut { get; private set; }
        [JsonProperty("paid_duration")]
        public int PaidDuration { get; private set; }
        [JsonProperty("placeholder")]
        public IJson Placeholder { get; private set; }
        [JsonProperty("can_publish_free_copy")]
        public bool CanPublishFreeCopy { get; private set; }
        [JsonProperty("edit_mode")]
        public DonutEditMode EditMode { get; private set; }

        private Donut()
        {
        }
    }
}
