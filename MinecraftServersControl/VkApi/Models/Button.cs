using PinkJson2;
using PinkJson2.Serializers;
using System;

namespace VkApi.Models
{
    public sealed class Button : IJsonDeserializable
    {
        [JsonProperty("action", DeserializerIgnore = true)]
        public ButtonAction Action { get; set; }
        [JsonProperty("color")]
        public ButtonColor? Color { get; set; }

        public void Deserialize(IDeserializer deserializer, IJson json)
        {
            deserializer.Deserialize(json, this, false);

            var actionJson = json["action"];
            var actionType = deserializer.Deserialize<ButtonActionType>(actionJson["type"]);

            switch (actionType)
            {
                case ButtonActionType.Text:
                    Action = deserializer.Deserialize<ButtonActionText>(actionJson);
                    break;
                case ButtonActionType.OpenLink:
                    Action = deserializer.Deserialize<ButtonActionOpenLink>(actionJson);
                    break;
                case ButtonActionType.Location:
                    Action = deserializer.Deserialize<ButtonActionLocation>(actionJson);
                    break;
                case ButtonActionType.Vkpay:
                    Action = deserializer.Deserialize<ButtonActionVkpay>(actionJson);
                    break;
                case ButtonActionType.OpenApp:
                    Action = deserializer.Deserialize<ButtonActionOpenApp>(actionJson);
                    break;
                case ButtonActionType.Callback:
                    Action = deserializer.Deserialize<ButtonActionCallback>(actionJson);
                    break;
            }
        }
    }
}
