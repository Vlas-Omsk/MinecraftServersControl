using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinecraftServersControl.DAL.Entities
{
    [Table("Session")]
    public sealed class Session
    {
        [Key]
        public byte[] Id { get; set; } = Guid.NewGuid().ToByteArray();
        public string UserLogin { get; set; }
        [ForeignKey("UserLogin")]
        public User User { get; set; }
        public int ExpiresAt { get; set; }
    }
}
