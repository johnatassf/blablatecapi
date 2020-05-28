using Blablatec.Domain.Dto;
using Blablatec.Domain.Model;
using Blablatec.Infra;
using Blablatec.Infra.Repositories;
using Blablatec.Infra.Services;
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
        private readonly IServiceInformationUser _servicoInformacaoUsuario;
        private readonly IServiceInformationUser _informacaoUsuario;

        public UserController(ILogger<UserController> logger,
            IRepository<Usuario> repositoryUser,
            IRepositoryUserManage repositoryUserManage,
            IServiceInformationUser servicoInformacaoUsuario,
            IServiceInformationUser informacaoUsuario)
        {
            _logger = logger;
            _repositoryUser = repositoryUser;
            _repositoryUserManage = repositoryUserManage;
            _servicoInformacaoUsuario = servicoInformacaoUsuario;
            _informacaoUsuario = informacaoUsuario;
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

        [HttpGet("getByRa")]
        public IActionResult GetByRa()
        {
            var usuarios = _repositoryUser.GetAll().Where(p => p.Ra == _informacaoUsuario.Ra);

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
            var usuarioLogado = _servicoInformacaoUsuario.Ra;



            var user =await _repositoryUserManage.UpdateProfile(usuario);

            return Ok(user);
        }

    }
}
