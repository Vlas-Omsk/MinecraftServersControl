using System;

namespace VkApi.Services
{
    public sealed class GroupsVkService : VkService
    {
        internal GroupsVkService(VkClient client) : base(client, "groups")
        {
        }

        public GroupsLongPollServer GetLongPollServer(int groupId)
        {
            return new GroupsLongPollServer(Client, groupId);
        }
    }
}
