using Blablatec.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Domain.Dto
{
    public class ViagemDtoSaida
    {
        
        public int Id { get; set; }
       
        public int IdMotorista { get; set; }
       
        public string PontoInicial { get; set; }
 
        public string PontoFinal { get; set; }
       
        public int? QtdLugares { get; set; }
 
        public double? Valor { get; set; }

        public DateTime? DataViagem { get; set; }

        public DateTime? Finalizacao { get; set; }

        public Usuario Motorista { get; set; }

        public bool MotoristaDaCorrida { get; set; }
        public int QuantidadeDeSolicitacaoAtiva { get; set; }
        public bool JaSolicitado { get; set; }
        public bool EmAndamento { get; set; } = false;
    }
}
