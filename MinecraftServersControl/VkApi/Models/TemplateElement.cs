using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class TemplateElement
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("photo_id")]
        public int PhotoId { get; set; }
        [JsonProperty("buttons")]
        public Button[] Buttons { get; set; }
        [JsonProperty("action")]
        public ButtonAction Action { get; set; }

        public TemplateElement()
        {
        }

        public TemplateElement(string title, string description, int photoId, Button[] buttons, ButtonAction action)
        {
            Title = title;
            Description = description;
            PhotoId = photoId;
            Buttons = buttons;
            Action = action;
        }
    }
}
