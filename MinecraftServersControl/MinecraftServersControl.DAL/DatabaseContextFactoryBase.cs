using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace MinecraftServersControl.DAL
{
    public abstract class DatabaseContextFactoryBase : IDbContextFactory<DatabaseContext>, IDisposable
    {
        protected static readonly string DatabaseRoot = Path.GetFullPath("../../../../../Database");

        private readonly DbContextOptions<DatabaseContext> _options;

        protected DatabaseContextFactoryBase()
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            OnConfiguration(builder);

            _options = builder.Options;

            OnLoaded();
        }

        protected virtual void OnConfiguration(DbContextOptionsBuilder<DatabaseContext> builder)
        {
        }

        protected virtual void OnLoaded()
        {
        }

        public DatabaseContext CreateDbContext()
        {
            return new DatabaseContext(_options);
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
