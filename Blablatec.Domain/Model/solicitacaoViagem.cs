using Blablatec.Domain.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blablatec.Domain.Model
{


    [Table("solicitacaoViagem")]
    public class solicitacaoViagem : IEntity
    {
        [Key]
        [Column("id_solicitacao_viagem")]
        public int Id { get; set; }
       
        [Column("id_usuario_carona")]
        public int IdUsuarioCarona { get; set; }
        [Column("id_viagem")]
        public int IdViagem { get; set; }
    }
}
