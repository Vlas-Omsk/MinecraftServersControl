using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinecraftServersControl.DAL.Entities
{
    [Table("VkUser")]
    public sealed class VkUser
    {
        [Key]
        public int Id { get; set; }
        public string UserLogin { get; set; }
        [ForeignKey("UserLogin")]
        public User User { get; set; }
    }
}
