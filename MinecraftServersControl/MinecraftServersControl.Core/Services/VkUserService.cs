using Microsoft.EntityFrameworkCore;
using MinecraftServersControl.Common;
using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Core.Interface.Services;
using MinecraftServersControl.DAL;
using MinecraftServersControl.DAL.Entities;
using MinecraftServersControl.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core.Services
{
    public sealed class VkUserService : Service, IVkUserService
    {
        internal VkUserService(Application application, DatabaseContextFactoryBase databaseContextFactory, Logger logger, IRemoteServer remoteServer) :
            base(application, databaseContextFactory, logger, remoteServer)
        {
        }

        public async Task<Result> SignIn(int vkUserId, string login, string password)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                var user = await databaseContext.Users
                    .FirstOrDefaultAsync(x => x.Login == login);
                if (user == null)
                    return ResultCode.UserNotFound;

                var passwordBytes = password.ToSha256HashBytes();

                if (user.PasswordHash.SequenceEqual(passwordBytes))
                {
                    await databaseContext.VkUsers.AddAsync(new VkUser() { Id = vkUserId, User = user });
                    await databaseContext.SaveChangesAsync();

                    return ResultCode.Success;
                }
            }

            return ResultCode.UserNotFound;
        }

        public async Task<Result<UserInfoDTO>> GetUserInfo(int vkUserId)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                var vkUser = await databaseContext.VkUsers
                    .FirstOrDefaultAsync(x => x.Id == vkUserId);

                if (vkUser == null)
                    return ResultCode.AccessDenied;

                return new UserInfoDTO(vkUser.UserLogin);
            }
        }

        public async Task<Result> CheckAccess(int vkUserId)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                var vkUser = await databaseContext.VkUsers
                    .AnyAsync(x => x.Id == vkUserId);

                return vkUser ? ResultCode.Success : ResultCode.AccessDenied;
            }
        }
    }
}
