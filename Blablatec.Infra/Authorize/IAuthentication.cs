using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blablatec.Infra.Authorize
{
    public interface IAuthentication
    {
        AuthenticationResult Authenticate(IUser user);
    }
}
