using System;

namespace MinecraftServersControl.Core.DTO
{
    [Serializable]
    public sealed class UserInfoDTO
    {
        public string Login { get; private set; }

        private UserInfoDTO()
        {
        }

        public UserInfoDTO(string login)
        {
            Login = login;
        }
    }
}
