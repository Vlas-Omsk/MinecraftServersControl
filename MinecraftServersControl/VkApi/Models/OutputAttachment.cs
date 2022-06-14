using PinkJson2.KeyTransformers;
using System;

namespace VkApi.Models
{
    public sealed class OutputAttachment
    {
        public AttachmentType Type { get; set; }
        public int OwnerId { get; set; }
        public int MediaId { get; set; }
        public string AccessKey { get; set; }

        public OutputAttachment(AttachmentType type)
        {
            Type = type;
        }

        public OutputAttachment(AttachmentType type, int ownerId, int mediaId, string accessKey)
        {
            Type = type;
            OwnerId = ownerId;
            MediaId = mediaId;
            AccessKey = accessKey;
        }

        public override string ToString()
        {
            var snakeCaseKeyTransformer = new SnakeCaseKeyTransformer();
            var result = $"{snakeCaseKeyTransformer.TransformKey(Enum.GetName(Type))}{OwnerId}_{MediaId}";
            if (AccessKey != null)
                result += '_' + AccessKey;
            return result;
        }
    }
}
