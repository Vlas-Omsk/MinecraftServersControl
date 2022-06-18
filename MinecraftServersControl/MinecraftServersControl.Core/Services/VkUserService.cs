using Microsoft.EntityFrameworkCore;
using MinecraftServersControl.Common;
using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Core.Abstractions;
using MinecraftServersControl.Core.Abstractions.Services;
using MinecraftServersControl.DAL;
using MinecraftServersControl.DAL.Entities;
using MinecraftServersControl.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using MinecraftServersControl.Remote.Server.Abstractions;

namespace MinecraftServersControl.Core.Services
{
    public sealed class VkUserService : Service, IVkUserService
    {
        internal VkUserService(Application application, DatabaseContextFactoryBase databaseContextFactory, Logger logger, IRemoteServer remoteServer) :
            base(application, databaseContextFactory, logger, remoteServer)
        {
        }

        public async Task SignIn(int vkUserId, string login, string password)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                var user = await databaseContext.Users
                    .FirstOrDefaultAsync(x => x.Login == login);
                if (user == null)
                    throw new CoreException(ErrorCode.UserNotFound);

                var passwordBytes = password.ToSha256HashBytes();

                if (user.PasswordHash.SequenceEqual(passwordBytes))
                {
                    await databaseContext.VkUsers.AddAsync(new VkUser() { Id = vkUserId, User = user });
                    await databaseContext.SaveChangesAsync();

                    return;
                }
            }

            throw new CoreException(ErrorCode.UserNotFound);
        }

        public async Task SignOut(int vkUserId)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                var vkUser = await databaseContext.VkUsers
                    .FirstOrDefaultAsync(x => x.Id == vkUserId);
                if (vkUser == null)
                    throw new CoreException(ErrorCode.UserNotFound);

                databaseContext.VkUsers.Remove(vkUser);
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task<UserInfoDTO> GetUserInfo(int vkUserId)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                var vkUser = await databaseContext.VkUsers
                    .FirstOrDefaultAsync(x => x.Id == vkUserId);

                if (vkUser == null)
                    throw new CoreException(ErrorCode.UserNotFound);

                return new UserInfoDTO(vkUser.UserLogin);
            }
        }

        public async Task<bool> IsAuthorized(int vkUserId)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                var vkUser = await databaseContext.VkUsers
                    .AnyAsync(x => x.Id == vkUserId);

                return vkUser;
            }
        }
    }
}
