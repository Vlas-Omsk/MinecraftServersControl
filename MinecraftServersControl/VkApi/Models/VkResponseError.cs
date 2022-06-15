using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class VkResponseError
    {
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }
        [JsonProperty("error_msg")]
        public string ErrorMessage { get; set; }
    }
}
