using Blablatec.Domain.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blablatec.Domain.Model
{
    [Table("Veiculo")]
    public class Carro: IEntity
    {
        [Key]
        [Column("id_veiculo")]
        public int Id { get; set; }
        [Column("id_placa_veiculo")] 

        public string Placa { get; set; }
        [Column("qtd_lugar_veiculo")] 
        public int QuantidadeLugares { get; set; }
        [Column("nm_modelo_veiculo")] 
        public string Marca { get; set; }
        [Column("nm_cor_veiculo")] 
        public string Cor { get; set; }
        [Column("id_usuario")]
        public int IdMotorista { get; set; }
    }
}
