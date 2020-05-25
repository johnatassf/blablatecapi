using Blablatec.Domain.Dto;
using Blablatec.Domain.Model;
using Blablatec.Infra;
using Blablatec.Infra.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blablatec.Controllers
{
    [ApiController]
    [Authorize]
    [Route("user")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IRepository<Usuario> _repositoryUser;
        private readonly IRepositoryUserManage _repositoryUserManage;

        public UserController(ILogger<UserController> logger,
            IRepository<Usuario> repositoryUser,
            IRepositoryUserManage repositoryUserManage)
        {
            _logger = logger;
            _repositoryUser = repositoryUser;
            _repositoryUserManage = repositoryUserManage;
        }

       
        [HttpGet]
        public IActionResult GetAll()
        {
            var usuarios = _repositoryUser.GetAll();

            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public IActionResult GetId([FromRoute] int id)
        {
            var usuarios = _repositoryUser.GetById(id);

            return Ok(usuarios);
        }

        [HttpPost()]
        public IActionResult CreateUser(Usuario usuario)
        {
            if (usuario == null)
                return BadRequest("Dados inválidos");
            
          //Fazer verificação do rm ou documento ja criado
        
            return Ok(_repositoryUser.Save(usuario));
        }

        [HttpPut("{id}/profile")]
        public async Task<IActionResult> UpdateProfile([FromRoute] int id, [FromBody] UpdateProfile usuario)
        {
            if (usuario.Id != id)
                return StatusCode(StatusCodes.Status409Conflict,
                  $"Id do usuario divergente do id informado");
            var usuarioLogado = User.Identity;



            var user =await _repositoryUserManage.UpdateProfile(usuario);

            return Ok(user);
        }

    }
}
