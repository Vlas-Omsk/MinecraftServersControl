using System;

namespace MinecraftServersControl.Core.DTO
{
    [Serializable]
    public sealed class SessionDTO
    {
        public Guid SessionId { get; private set; }
        public DateTime ExpiresAt { get; private set; }

        private SessionDTO()
        {
        }

        public SessionDTO(Guid sessionId, DateTime expiresAt)
        {
            SessionId = sessionId;
            ExpiresAt = expiresAt;
        }
    }
}
