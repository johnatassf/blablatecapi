using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Infra.Authorize
{
    public class LoginUser : IUser
    {
        public string Ra { get; set ; }
        public string Password { get ; set ; }
    }
}
