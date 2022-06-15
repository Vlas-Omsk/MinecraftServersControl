using Microsoft.EntityFrameworkCore;
using MinecraftServersControl.Common;
using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Core.Interface;
using MinecraftServersControl.Core.Interface.Services;
using MinecraftServersControl.DAL;
using MinecraftServersControl.DAL.Entities;
using MinecraftServersControl.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core.Services
{
    public sealed class UserService : Service, IUserService
    {
        private static readonly TimeSpan _sessionLifeTime = TimeSpan.FromDays(1);

        internal UserService(Application application, DatabaseContextFactoryBase databaseContextFactory, Logger logger, IRemoteServer remoteServer) : 
            base(application, databaseContextFactory, logger, remoteServer)
        {
        }

        public async Task<SessionDTO> SignIn(UserDTO userSchema)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                var user = await databaseContext.Users
                    .FirstOrDefaultAsync(x => x.Login == userSchema.Login);
                if (user == null)
                    throw new CoreException(ErrorCode.UserNotFound);

                var passwordBytes = userSchema.PasswordHash.ToByteArray();

                if (user.PasswordHash.SequenceEqual(passwordBytes))
                    return await CreateSession(userSchema.Login);
            }

            throw new CoreException(ErrorCode.UserNotFound);
        }

        public async Task<SessionDTO> Restore(Guid sessionId)
        {
            var session = await GetSessionById(sessionId);

            if (!VerifySession(session))
                throw new CoreException(ErrorCode.SessionExpired);

            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                databaseContext.Sessions.RemoveRange(session);
                await databaseContext.SaveChangesAsync();
            }

            RaiseSessionRemoved(sessionId);

            return await CreateSession(session.UserLogin);
        }

        public async Task VerifySession(Guid sessionId)
        {
            var session = await GetSessionById(sessionId);
            if (!VerifySession(session))
                throw new CoreException(ErrorCode.SessionExpired);
        }

        private static bool VerifySession(Session session)
        {
            return session != null && DateTime.Now.ToUnixTime() < session.ExpiresAt;
        }

        private Task<Session> GetSessionById(Guid sessionId)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
                return databaseContext.Sessions
                    .FirstOrDefaultAsync(x =>
                        x.Id.SequenceEqual(sessionId.ToByteArray())
                    );
        }

        private async Task<SessionDTO> CreateSession(string login)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                var session = new Session()
                {
                    UserLogin = login,
                    ExpiresAt = (int)(DateTime.Now + _sessionLifeTime).ToUnixTime()
                };
                await databaseContext.Sessions.AddAsync(session);
                await databaseContext.SaveChangesAsync();

                return session.ToSessionDTO();
            }
        }

        private void RaiseSessionRemoved(Guid sessionId)
        {
            SessionRemoved?.Invoke(this, sessionId);
        }

        public event EventHandler<Guid> SessionRemoved;
    }
}
