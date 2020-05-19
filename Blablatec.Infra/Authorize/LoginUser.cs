using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Infra.Authorize
{
    public class LoginUser : IUser
    {
        public string Id { get; set ; }
        public string Name { get ; set ; }
    }
}
