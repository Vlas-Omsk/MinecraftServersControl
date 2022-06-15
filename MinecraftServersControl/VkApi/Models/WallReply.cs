using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class WallReply
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("from_id")]
        public int FromId { get; set; }
        [JsonProperty("date")]
        public int Date { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("donut")]
        public WallReplyDonut Donut { get; set; }
        [JsonProperty("reply_to_user")]
        public int ReplyToUser { get; set; }
        [JsonProperty("reply_to_comment")]
        public int ReplyToComment { get; set; }
        [JsonProperty("attachments")]
        public Attachment[] Attachments { get; set; }
        [JsonProperty("parents_stack")]
        public int[] ParentsStack { get; set; }
        [JsonProperty("thread")]
        public VkThread Thread { get; set; }
        [JsonProperty("post_id")]
        public int PostId { get; set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }
    }
}
