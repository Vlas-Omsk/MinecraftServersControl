using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Template
    {
        [JsonProperty("type")]
        public TemplateType Type { get; set; }
        [JsonProperty("elements")]
        public TemplateElement[] Elements { get; set; }

        public Template(TemplateType type, TemplateElement[] elements)
        {
            Type = type;
            Elements = elements;
        }
    }
}
