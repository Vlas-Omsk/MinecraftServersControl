using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinecraftServersControl.DAL.Entities
{
    [Table("Computer")]
    public sealed class Computer
    {
        [Key]
        public byte[] Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public byte[] IpAddress { get; set; }
        public byte[] MacAddress { get; set; }
    }
}
