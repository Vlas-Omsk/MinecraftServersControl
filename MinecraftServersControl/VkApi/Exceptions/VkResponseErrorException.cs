using System;
using VkApi.Models;

namespace VkApi
{
    public sealed class VkResponseErrorException : Exception
    {
        public VkResponseError Error { get; }

        public VkResponseErrorException(VkResponseError error) : base($"{error.ErrorMessage} (Code: {error.ErrorCode})")
        {
        }
    }
}
