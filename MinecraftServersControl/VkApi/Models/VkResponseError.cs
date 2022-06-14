using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class VkResponseError
    {
        [JsonProperty("error_code")]
        public int ErrorCode { get; private set; }
        [JsonProperty("error_msg")]
        public string ErrorMessage { get; private set; }

        private VkResponseError()
        {
        }
    }
}
