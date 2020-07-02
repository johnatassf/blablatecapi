using Blablatec.Domain.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blablatec.Domain.Model
{
    [Table("solicitacaoviagem")]
    public class solicitacaoViagem : IEntity
    {
        [Key]
        [Column("id_solicitacao_viagem")]
        public int Id { get; set; }
        [Column("id_usuario_carona")]
        public int IdUsuario { get; set; }
        [ForeignKey("IdUsuario")]
        public Usuario Carona { get; set; }
        [Column("id_viagem")]
        public int IdViagem { get; set; }
        [ForeignKey("IdViagem")]
        public Viagem Viagem { get; set; }

        [Column("recusada")]
        public bool? Recusada { get; set; }
    }
}
