using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class WallReply
    {
        [JsonProperty("id")]
        public int Id { get; private set; }
        [JsonProperty("from_id")]
        public int FromId { get; private set; }
        [JsonProperty("date")]
        public int Date { get; private set; }
        [JsonProperty("text")]
        public string Text { get; private set; }
        [JsonProperty("donut")]
        public WallReplyDonut Donut { get; private set; }
        [JsonProperty("reply_to_user")]
        public int ReplyToUser { get; private set; }
        [JsonProperty("reply_to_comment")]
        public int ReplyToComment { get; private set; }
        [JsonProperty("attachments")]
        public Attachment[] Attachments { get; private set; }
        [JsonProperty("parents_stack")]
        public int[] ParentsStack { get; private set; }
        [JsonProperty("thread")]
        public VkThread Thread { get; private set; }
        [JsonProperty("post_id")]
        public int PostId { get; private set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; private set; }

        private WallReply()
        {
        }
    }
}
