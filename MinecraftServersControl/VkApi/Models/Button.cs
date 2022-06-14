using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Button
    {
        [JsonProperty("title")]
        public string Title { get; private set; }
        [JsonProperty("action")]
        public ButtonAction Action { get; private set; }
        [JsonProperty("color")]
        public ButtonColor Color { get; private set; }

        public Button()
        {
        }

        public Button(string title, ButtonAction action, ButtonColor color)
        {
            Title = title;
            Action = action;
            Color = color;
        }
    }
}
