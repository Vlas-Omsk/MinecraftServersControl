using PinkJson2;
using PinkJson2.Formatters;
using System;
using VkApi;

namespace MinecraftServersControl.Tests
{
    internal sealed class VkLongPollTest
    {
        public void Start()
        {
            var client = new VkClient("b4c3172aa829a9d7b2ea0cc83710637a4bdc34207bc13736b802ef0db6c438becf80c7cf6970942a2b15c");
            var longPollServer = new GroupsLongPollServer(client, 213893484);
            longPollServer.Update += OnLongPollServerUpdate;
            longPollServer.Start();

            while (true)
            {
                Console.ReadLine();
            }
        }

        private static void OnLongPollServerUpdate(object sender, LongPollUpdateEventArgs e)
        {
            Console.WriteLine(e.Updates.Serialize(VkClient.ObjectSerializerOptions).ToString(new PrettyFormatter()));
        }
    }
}
