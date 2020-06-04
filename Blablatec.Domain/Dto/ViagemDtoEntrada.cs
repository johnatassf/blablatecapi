using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Domain.Dto
{
    public class ViagemDtoEntrada
    {
        public double Valor { get; set; }
        public DateTime Viagem { get; set; }
        public string PontoInicial { get; set; }
        public string PontoFinal { get; set; }
        public int QtdLugares { get; set; }
    }
}
