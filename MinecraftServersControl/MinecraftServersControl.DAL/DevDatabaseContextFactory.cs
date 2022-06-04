using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace MinecraftServersControl.DAL
{
    public sealed class DevDatabaseContextFactory : DatabaseContextFactoryBase
    {
        private const string _databaseFileName = "test.sqlite3";

        protected override void OnConfiguration(DbContextOptionsBuilder<DatabaseContext> builder)
        {
            builder.UseSqlite($"Data Source={_databaseFileName};");
        }

        protected override void OnLoaded()
        {
            RemoveDatabaseFile();

            var path = Path.Combine(DatabaseRoot, "db.sql");
            var query = File.ReadAllText(path);

            using (var databaseContext = CreateDbContext())
            {
                databaseContext.Database.ExecuteSqlRaw(query);
            }
        }

        private void RemoveDatabaseFile()
        {
            foreach (var file in new DirectoryInfo(".")
                    .EnumerateFiles()
                    .Where(x => x.Name.StartsWith(_databaseFileName)))
                file.Delete();
        }

        public override void Dispose()
        {
            RemoveDatabaseFile();

            base.Dispose();
        }
    }
}
