using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(string))]
    public enum MessageActionType
    {
        ChatPhotoUpdate,
        ChatPhotoRemove,
        ChatCreate,
        ChatTitleUpdate,
        ChatInviteUser,
        ChatKickUser,
        ChatPinMessage,
        ChatUnpinMessage,
        ChatInviteUserByLink
    }
}
