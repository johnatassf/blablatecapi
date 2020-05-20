using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Blablatec.Domain.Model
{
    [Table("Usuario")]
    public class Usuario: IEntity
    {
        [Column("id_usuario")]
        public int Id { get; set; }
        [Column("nm_email_usuario")]
        public string Email { get; set; }
        [Column("nm_usuario")]
        public string Nome { get; set; }
        [Column("nm_sobrenome_usuario")]
        public string Sobrenome { get; set; }
        [Column("cd_ra_usuario")]
        public string Ra { get; set; }
        [NotMapped]
        [JsonIgnore]
        public byte[] Passwordhash { get; set; }
        [NotMapped]
        [JsonIgnore]
        public byte[] Passwordsalt { get; set; }
    }
}
