using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace MinecraftServersControl.DAL
{
    public sealed class DatabaseContextFactory : DatabaseContextFactoryBase
    {
        protected override void OnConfiguration(DbContextOptionsBuilder<DatabaseContext> builder)
        {
            var path = Path.Combine(DatabaseRoot, "db.sqlite3");

            builder.UseSqlite($"Data Source={path};");
        }
    }
}
