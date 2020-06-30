using Blablatec.Domain.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blablatec.Domain.Model
{
    [Table("rotaAtiva")]
    public class RotaAtiva : IEntity
    {
        [Column("id_rotas_ativas")]
        public int Id { get; set; }
       
        [Column("cd_latitude_atual")]
        public string LatitudeAtual { get; set; }
       
        [Column("cd_longitude_atual")]
        public string LongitudeAtual { get; set; }
       
        [Column("id_viagem")] 
        public int IdViagem { get; set; }
       
        [ForeignKey("IdViagem")]
        public Viagem Viagem { get; set; }
    }
}
