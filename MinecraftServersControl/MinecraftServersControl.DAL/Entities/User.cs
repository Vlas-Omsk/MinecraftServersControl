using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinecraftServersControl.DAL.Entities
{
    [Table("User")]
    public sealed class User
    {
        [Key]
        public string Login { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}
