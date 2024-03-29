﻿using PinkJson2;
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
        public string PhotoId { get; set; }
        [JsonProperty("buttons")]
        public Button[] Buttons { get; set; }
        [JsonProperty("action")]
        public TemplateElementAction Action { get; set; }

        public TemplateElement()
        {
        }

        public TemplateElement(string title, string description, string photoId, Button[] buttons, TemplateElementAction action)
        {
            Title = title;
            Description = description;
            PhotoId = photoId;
            Buttons = buttons;
            Action = action;
        }
    }
}
