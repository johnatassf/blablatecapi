using Blablatec.Domain.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blablatec.Domain.Model
{
    [Table("itemviagem")]
    public class ItemViagem: IEntity
    {

        [Column("id_item_viagem")]
        public int Id { get; set; }
        [Column("vl_viagem")]
        public double Valor { get; set; }
        [Column("id_usuario_motorista")]
        public int IdMotorista { get; set; }
        [Column("dt_viagem")]
        public DateTime Viagem { get; set; }
        [Column("dt_finalizacao")]
        public DateTime Finalizacao { get; set; }
    }
}
