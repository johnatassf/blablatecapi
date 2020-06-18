using Blablatec.Domain.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blablatec.Domain.Model
{
    [Table("viagem")]
    public class Viagem : IEntity
    {
        [Column("id_viagem")]
        public int Id { get; set; }
        [Column("id_usuario_motorista")]
        public int IdMotorista { get; set; }
        [Column("nm_origem_viagem")]
        public string PontoInicial { get; set; }

        [Column("nm_destino_viagem")]
        public string PontoFinal { get; set; }

        [Column("qtd_lugar_disponivel")]
        public int? QtdLugares { get; set; }

        [Column("vl_viagem")]
        public double? Valor { get; set; }

        [Column("dt_viagem")]
        public DateTime? DataViagem { get; set; }

        [Column("dt_finalizacao")]
        public DateTime? Finalizacao { get; set; }

        [ForeignKey("IdMotorista")]
        public Usuario Motorista { get; set; }

    }
}
