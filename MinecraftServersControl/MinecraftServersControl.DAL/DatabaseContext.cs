using Microsoft.EntityFrameworkCore;
using MinecraftServersControl.DAL.Entities;
using System;

namespace MinecraftServersControl.DAL
{
    public sealed class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; private set; }
        public DbSet<VkUser> VkUsers { get; private set; }
        public DbSet<Session> Sessions { get; private set; }
        public DbSet<Computer> Computers { get; private set; }
        public DbSet<Server> Servers { get; private set; }

        internal DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
    }
}
