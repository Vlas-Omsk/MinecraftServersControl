using System;
using System.Threading.Tasks;
using VkApi.Models;

namespace VkApi.Services
{
    public sealed class MessagesVkService : VkService
    {
        internal MessagesVkService(VkClient client) : base(client, "messages")
        {
        }

        public async Task<MessagesSendResult[]> Send(
            int? userId = null, int? randomId = null, int? peerId = null,
            int[] peerIds = null, string domain = null, int? chatId = null,
            int[] userIds = null, string message = null, int? guid = null,
            int? latitude = null, int? longitude = null, OutputAttachment[] attachments = null,
            int? replyTo = null, int[] forwardMessages = null, OutputMessageForward forward = null,
            int? stickerId = null, int? groupId = null, Keyboard keyboard = null, Template template = null,
            string payload = null, ContentSource contentSource = null, bool? dontParseLinks = null,
            bool? disableMentions = null, string intent = null, int? subscribeId = null
        )
        {
            var request = CreateRequest("send");
            request.SetParameter("user_id", userId);
            request.SetParameter("random_id", randomId);
            request.SetParameter("peer_id", peerId);
            request.SetParameter("peer_ids", peerIds);
            request.SetParameter("domain", domain);
            request.SetParameter("chat_id", chatId);
            request.SetParameter("user_ids", userIds);
            request.SetParameter("message", message);
            request.SetParameter("guid", guid);
            request.SetParameter("lat", latitude);
            request.SetParameter("long", longitude);
            request.SetParameter("attachment", attachments);
            request.SetParameter("reply_to", replyTo);
            request.SetParameter("forward_messages", forwardMessages);
            request.SetJsonParameter("forward", forward);
            request.SetParameter("sticker_id", stickerId);
            request.SetParameter("group_id", groupId);
            request.SetJsonParameter("keyboard", keyboard);
            request.SetJsonParameter("template", template);
            request.SetParameter("payload", payload);
            request.SetJsonParameter("content_source", contentSource);
            request.SetParameter("dont_parse_links", dontParseLinks);
            request.SetParameter("disable_mentions", disableMentions);
            request.SetParameter("intent", intent);
            request.SetParameter("subscribe_id", subscribeId);

            if (peerIds == null)
                return new MessagesSendResult[]
                {
                    new MessagesSendResult()
                    {
                        MessageId = await request.GetResponse<int>()
                    }
                };
            else
                return await request.GetResponse<MessagesSendResult[]>();
        }
    }
}
