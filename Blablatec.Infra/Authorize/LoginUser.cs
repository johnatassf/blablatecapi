using Blablatec.Domain.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Infra.Authorize
{
    public class LoginUser : IUser
    {
        public int Id { get; set; }
        public string Ra { get; set ; }
        public string Password { get ; set ; }
    }
}
