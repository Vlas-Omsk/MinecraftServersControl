using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Donut
    {
        [JsonProperty("is_donut")]
        public bool IsDonut { get; set; }
        [JsonProperty("paid_duration")]
        public int PaidDuration { get; set; }
        [JsonProperty("placeholder")]
        public IJson Placeholder { get; set; }
        [JsonProperty("can_publish_free_copy")]
        public bool CanPublishFreeCopy { get; set; }
        [JsonProperty("edit_mode")]
        public DonutEditMode EditMode { get; set; }
    }
}
