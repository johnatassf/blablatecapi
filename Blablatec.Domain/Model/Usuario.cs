using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Blablatec.Domain.Interface;

namespace Blablatec.Domain.Model
{
    [Table("Usuario")]
    public class Usuario: IEntity, IUser
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
        [Column("nr_telefone")]
        public string NumeroTelefone { get; set; }

        [JsonIgnore]
        [Column("cd_passwordhash")]
        public byte[] Passwordhash { get; set; }
       
        [JsonIgnore]
        [Column("cd_passwordsalt")]
        public byte[] Passwordsalt { get; set; }
    }
}
