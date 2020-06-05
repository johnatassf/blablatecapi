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
        private readonly IRepository<Usuario> _repositoryUsuario;

        //private readonly IServiceAuthorization _serviceAuthorization;

        public AuthorizeController(
            IAuthentication serviceAuthentication,
            IRepositoryUserManage repositoryUserManage,
            IRepository<Usuario> repositoryUsuario)
        {
            _serviceAuthentication = serviceAuthentication;
            _repositoryUserManage = repositoryUserManage;
            _repositoryUsuario = repositoryUsuario;
            //_serviceAuthorization = serviceAuthorization;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUser loginUser)
        {
            var user = _repositoryUserManage.Authorize(loginUser);

            if (user == null)
            {
                return BadRequest(new BaseResult<Object> { Message = "401 - Não autorizado", Success = false });
            }

            var authentication = _serviceAuthentication.Authenticate(user);

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

        [AllowAnonymous]
        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassWord(string ra, string email)
        {
           var user = await _repositoryUsuario.GetOne(u => u.Ra == ra && u.Email == email);
            
            if (user == null)
                return BadRequest("Ra\\Email não cadatrado");

            await  _repositoryUserManage.UpdatePassword(user);

            return NoContent();
        }



    }
}
