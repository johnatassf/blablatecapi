using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Domain.Dto
{
    public class UpdateProfile
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string NumeroTelefone { get; set; }
        public string Ra { get; set; }

    }
}
