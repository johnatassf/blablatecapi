using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Domain.Model
{
    public class Usuario: IEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Ra { get; set; }

    }
}
