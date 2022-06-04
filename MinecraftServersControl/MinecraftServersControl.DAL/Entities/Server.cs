using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinecraftServersControl.DAL.Entities
{
    [Table("Server")]
    public sealed class Server
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ComputerId { get; set; }
        [ForeignKey("ComputerId")]
        public Computer Computer { get; set; }
    }
}
