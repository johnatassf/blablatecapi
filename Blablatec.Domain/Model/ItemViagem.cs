using Blablatec.Domain.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blablatec.Domain.Model
{
    [Table("itemviagem")]
    public class ItemViagem : IEntity
    {

        [Column("id_item_viagem")]
        public int Id { get; set; }

        [Column("id_usuario_carona")]
        public int IdUsuarioCarona { get; set; }

        [Column("id_viagem")]
        public int IdViagem { get; set; }

        [ForeignKey("IdUsuarioCarona")]
        public Usuario UsuarioCarona { get; set; }

        [ForeignKey("IdViagem")]
        public Viagem Viagem { get; set; }

    }
}
