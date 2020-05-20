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
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Ra { get; set; }
        [JsonIgnore]
        public byte[] Passwordhash { get; set; }
        [JsonIgnore]
        public byte[] Passwordsalt { get; set; }
    }
}
