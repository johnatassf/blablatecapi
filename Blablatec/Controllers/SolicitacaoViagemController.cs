using Blablatec.Domain.Model;
using Blablatec.Infra.Repositories;
using Blablatec.Infra.Services;
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
    [Route("solicitacaoViagem")]
    public class SolicitacaoViagemController : ControllerBase
    {
        private readonly ILogger<SolicitacaoViagemController> _logger;
        private readonly IRepository<SolicitacaoViagem> _repositorySolicitacaoViagem;
        private readonly IServiceInformationUser _servicoInformacaoUsuario;

        public SolicitacaoViagemController(ILogger<SolicitacaoViagemController> logger,
            IRepository<SolicitacaoViagem> repositoryCarro,
            IServiceInformationUser servicoInformacaoUsuario)
        {
            _logger = logger;
            _repositorySolicitacaoViagem = repositoryCarro;
            _servicoInformacaoUsuario = servicoInformacaoUsuario;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var carros = _repositorySolicitacaoViagem.GetEntityByExpression(null,p => p.Viagem, v => v.Carona);

            return Ok(carros);
        }

        [HttpGet("{id}")]
        public IActionResult GetId([FromRoute] int id)
        {
            var carros = _repositorySolicitacaoViagem.GetById(id);

            return Ok(carros);
        }

        [HttpPost()]
        public IActionResult CreateSolicitacaoViagem(SolicitacaoViagem solicitacaoViagem)
        {
            if (solicitacaoViagem == null)
                return BadRequest("Dados inválidos");

            //Fazer verificação do rm ou documento ja criado

            return Ok(_repositorySolicitacaoViagem.Save(solicitacaoViagem));
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarSolicitacaoViagem([FromRoute] int id, SolicitacaoViagem solicitacaoViagem)
        {
            if (solicitacaoViagem.Id != id)
                return StatusCode(StatusCodes.Status409Conflict,
                  $"Id da solicitação de viagem divergente do id informado");

            var user = _repositorySolicitacaoViagem.Update(solicitacaoViagem);

            return Ok(user);
        }
    }
}
