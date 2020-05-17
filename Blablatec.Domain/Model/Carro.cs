using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blablatec.Domain.Model
{
    [Table("Carro")]
    public class Carro: IEntity
    {
        public int Id { get; set; }
        public string Placa { get; set; }
        public int QuantidadeLugares { get; set; }
        public string Marca { get; set; }
        public string Cor { get; set; }
    }
}
