using System;

namespace MinecraftServersControl.API.Vk
{
    public sealed class VerifyResult
    {
        public string Message { get; }

        public VerifyResult(string message)
        {
            Message = message;
        }
    }
}
