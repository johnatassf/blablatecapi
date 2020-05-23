using Blablatec.Domain.Model;
using Blablatec.Infra;
using Blablatec.Infra.Repositories;
using Microsoft.AspNetCore.Authorization;
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
    [Route("user")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IRepository<Usuario> _repositoryUser;
        private readonly ContextBlablatec _contextBlablatec;

        public UserController(ILogger<UserController> logger,
            IRepository<Usuario> repositoryUser)
        {
            _logger = logger;
            _repositoryUser = repositoryUser;
        }

        [Authorize]
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

        [HttpPut("{id}")]
        public IActionResult AtualizarUser([FromRoute] int id, Usuario usuario)
        {
            if (usuario.Id != id)
                return StatusCode(StatusCodes.Status409Conflict,
                  $"Id do usuario divergente do id informado");
            
            var user = _repositoryUser.Update(usuario);

            return Ok(user);
        }

    }
}
