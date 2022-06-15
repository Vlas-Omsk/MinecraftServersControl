using PinkJson2;
using PinkJson2.Serializers;
using System;

namespace VkApi.Models
{
    public sealed class LongPollUpdate : IJsonDeserializable
    {
        [JsonProperty("type")]
        public LongPollUpdateType Type { get; set; }
        [JsonProperty("event_id")]
        public string EventId { get; set; }
        [JsonProperty("v")]
        public string Version { get; set; }
        [JsonProperty("object", DeserializerIgnore = true)]
        public object Object { get; set; }
        [JsonProperty("group_id")]
        public int GroupId { get; set; }

        void IJsonDeserializable.Deserialize(IDeserializer deserializer, IJson json)
        {
            deserializer.Deserialize(json, this, false);

            var objectJson = json["object"];

            switch (Type)
            {
                case LongPollUpdateType.MessageNew:
                    Object = deserializer.Deserialize<LongPollMessageNew>(objectJson);
                    break;
            }
        }
    }
}
