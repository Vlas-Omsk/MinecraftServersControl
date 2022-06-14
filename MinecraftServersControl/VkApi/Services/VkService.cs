using System;

namespace VkApi.Services
{
    public abstract class VkService
    {
        protected VkClient Client { get; }

        private string _serviceName;

        protected VkService(VkClient client, string serviceName)
        {
            Client = client;
            _serviceName = serviceName;
        }

        protected VkRequest CreateRequest(string method)
        {
            return Client.CreateRequest(_serviceName, method);
        }
    }
}
