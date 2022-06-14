using System;

namespace MinecraftServersControl.Core.DTO
{
    [Serializable]
    public sealed class UserDTO 
    {
        public string Login { get; private set; }
        public string PasswordHash { get; private set; }

        private UserDTO()
        {
        }

        public UserDTO(string login, string passwordHash)
        {
            Login = login;
            PasswordHash = passwordHash;
        }
    }
}
