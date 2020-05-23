using Blablatec.Domain.Dto;
using Blablatec.Domain.Model;
using Blablatec.Infra.Authorize;
using Blablatec.Infra.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blablatec.Controllers
{
    [ApiController]
    [Route("user")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IAuthentication _serviceAuthentication;
        private readonly IRepositoryUserManage _repositoryUserManage;

        //private readonly IServiceAuthorization _serviceAuthorization;

        public AuthorizeController(
            IAuthentication serviceAuthentication,
            IRepositoryUserManage repositoryUserManage )
        {
            _serviceAuthentication = serviceAuthentication;
            _repositoryUserManage = repositoryUserManage;
            //_serviceAuthorization = serviceAuthorization;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUser loginUser)
        {
            var authorization = _repositoryUserManage.Authorize(loginUser);

            if (!authorization)
            {
                return BadRequest(new BaseResult<Object> { Message = "401 - Não autorizado", Success = false });
            }

            var authentication = _serviceAuthentication.Authenticate(loginUser);

            if (!authentication.Success)
            {
                return BadRequest(new BaseResult<Object> {Message = "401 - Não autorizado", Success= false });
            }

            return Ok(authentication);
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] RegistroUsuarioDto user)
        {
            if (user == null)
                return BadRequest("Dados inválidos");

            var userCreated = _repositoryUserManage.RegisterUser(user);

            return Created("", userCreated);
        }



    }
}
