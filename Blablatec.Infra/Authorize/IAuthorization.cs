using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blablatec.Infra.Authorize
{
    public interface IAuthorization
    {
        BaseResult<IUser> AuthorizeAsync(LoginUser loginUser);
    }
}
