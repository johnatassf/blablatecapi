using Blablatec.Domain.Dto;
using Blablatec.Domain.Model;
using Blablatec.Infra.Authorize;

namespace Blablatec.Infra.Repositories
{
    public interface IRepositoryUserManage
    {
        BaseResult<Usuario> RegisterUser(RegistroUsuarioDto user);
        bool Authorize(LoginUser loginUser);

    }
}