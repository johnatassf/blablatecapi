using Blablatec.Domain.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blablatec.Domain.Model
{
    [Table("rotasAtivas")]
    public class RotaAtiva : IEntity
    {
        [Column("id_rotas_ativas")]
        public int Id { get; set; }
        [Column("cd_latitude_atual")]
        public string LatitudeAtual { get; set; }
        [Column("cd_longitude_atual")]
        public string LongitudeAtual { get; set; }
        [Column("id_item_viagem")] 
        public int IdItemViagem { get; set; }
        [ForeignKey("IdItemViagem")]
        public ItemViagem? Viagem { get; set; }
    }
}
