using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Domain.Dto
{
    public class RegistroUsuarioDto
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Ra { get; set; }
        public bool Motorista { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public string CorCarro { get; set; }
        public int? QtsLugares { get; set; }
        public string NumeroTelefone { get; set; }
    }
}
