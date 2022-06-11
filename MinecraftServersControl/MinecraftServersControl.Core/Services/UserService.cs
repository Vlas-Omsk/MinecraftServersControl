using Microsoft.EntityFrameworkCore;
using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.DAL;
using MinecraftServersControl.DAL.Entities;
using MinecraftServersControl.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core.Services
{
    public sealed class UserService : Service
    {
        private static readonly TimeSpan _sessionLifeTime = TimeSpan.FromDays(1);

        internal UserService(Application application, DatabaseContextFactoryBase databaseContextFactory, Logger logger) : 
            base(application, databaseContextFactory, logger)
        {
        }

        public async Task<Result<SessionDTO>> SignIn(UserDTO userSchema)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                var user = await databaseContext.Users
                    .FirstOrDefaultAsync(x => x.Login == userSchema.Login);
                if (user == null)
                    return ResultCode.UserNotFound;

                var passwordBytes = userSchema.PasswordHash.ToByteArray();

                if (user.PasswordHash.SequenceEqual(passwordBytes))
                    return await CreateSession(userSchema.Login);
            }

            return ResultCode.UserNotFound;
        }

        public async Task<Result<SessionDTO>> Restore(Guid sessionId)
        {
            var session = await GetSessionById(sessionId);

            if (!VerifySession(session))
                return ResultCode.SessionExpired;

            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                databaseContext.Sessions.RemoveRange(session);
                await databaseContext.SaveChangesAsync();
            }

            RaiseSessionRemoved(sessionId);

            return await CreateSession(session.UserLogin);
        }

        public async Task<Result> VerifySession(Guid sessionId)
        {
            var session = await GetSessionById(sessionId);

            if (!VerifySession(session))
                return ResultCode.SessionExpired;

            return ResultCode.Success;
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

        private async Task<Result<SessionDTO>> CreateSession(string login)
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
            SessionRemoved?.Invoke(this, new Result<Guid>(sessionId, ResultCode.AuthorizationFromAnotherPlace));
        }

        public event ResultEventHandler<Guid> SessionRemoved;
    }
}
