using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Domain.Dto
{
    public class RotaAtivaDtoSaida
    {
        public int Id { get; set; }
        public string LatLng { get; set; }
        public int idMotorista { get; set; }
        public int IdUsuarioLogado { get; set; }
        public int IdViagem { get; set; }
        public bool IsMotorista { get; set; }
        public string PontoFinal { get; set; }
    }
}
