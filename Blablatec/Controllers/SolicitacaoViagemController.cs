using Blablatec.Domain.Model;
using Blablatec.Infra.Repositories;
using Blablatec.Infra.Services;
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
    [Authorize]
    [ApiController]
    [Route("solicitacao-viagem")]
    public class SolicitacaoViagemController : ControllerBase
    {
        private readonly ILogger<SolicitacaoViagemController> _logger;
        private readonly IRepository<SolicitacaoViagem> _repositorySolicitacaoViagem;
        private readonly IRepository<ItemViagem> _repositoryItemViagem;
        private readonly IRepository<Viagem> _repositoryViagem;
        private readonly int _idUsuarioLogado;
        private readonly IServiceInformationUser _serviceInformationUser;

        public SolicitacaoViagemController(ILogger<SolicitacaoViagemController> logger,
            IRepository<SolicitacaoViagem> repositorySolicitacaoViagem,
            IRepository<ItemViagem> repositoryItemViagem,
            IRepository<Viagem> repositoryViagem,
            IServiceInformationUser serviceInformationUser)
        {
            _logger = logger;
            _repositorySolicitacaoViagem = repositorySolicitacaoViagem;
            _repositoryItemViagem = repositoryItemViagem;
            _repositoryViagem = repositoryViagem;
            _idUsuarioLogado = Convert.ToInt32(serviceInformationUser.IdUsuario);
            _serviceInformationUser = serviceInformationUser;
        }

        [HttpGet("getAll/{idViagem}")]
        public IActionResult GetAll([FromRoute] int idViagem)
        {
            var carros = _repositorySolicitacaoViagem.GetEntityByExpression(p => p.Viagem.Id == idViagem && p.Recusada == null, v=> v.Viagem, v => v.Carona );

            return Ok(carros);
        }

        [HttpGet("{id}")]
        public IActionResult GetId([FromRoute] int id)
        {
            var carros = _repositorySolicitacaoViagem.GetById(id);

            return Ok(carros);
        }

  
        [HttpPost("viagem/{idViagem}")]
        public IActionResult CriarSolicitacaoViagem([FromRoute]int idViagem)
        {
            var viagem = _repositoryViagem.GetById(idViagem);

            if (viagem == null)
                return BadRequest("Viagem não encontrada");

            if (viagem.Finalizacao != null)
                return BadRequest("Viagem ja finalizada");

            if (viagem.IdMotorista == _idUsuarioLogado)
                return BadRequest("Usuário motorista não pode solicitar carona para sua própria corrida");

            var solicitacaoViagem = new SolicitacaoViagem()
            {
                IdUsuario = _idUsuarioLogado,
                IdViagem = viagem.Id
            };

            return Ok(_repositorySolicitacaoViagem.Save(solicitacaoViagem));
        }

        [HttpDelete("viagem/{idViagem}")]
        public async Task<IActionResult> RemoverSolicitacaoViagem([FromRoute]int idViagem)
        {

            var viagem = _repositoryViagem.GetById(idViagem);

            if (viagem == null)
                return BadRequest("Viagem não encontrada");

            if (viagem.Finalizacao != null)
                return BadRequest("Viagem ja finalizada");

            var solicitacao = await _repositorySolicitacaoViagem.GetOne(s => s.IdUsuario == _idUsuarioLogado && s.IdViagem == idViagem);

            if (solicitacao == null)
                return BadRequest("Solicitacao viagem não encontrada");

            _repositorySolicitacaoViagem.Remove(solicitacao);
            
            return NoContent();
        }

        [HttpPut("{id}/profile")]
        public IActionResult Update([FromRoute] int id, [FromBody] SolicitacaoViagem solicitacao)
        {
            if (solicitacao.Id != id)
                return StatusCode(StatusCodes.Status409Conflict,
                  $"Id do usuario divergente do id informado");

            _repositorySolicitacaoViagem.Update(solicitacao);

            if (solicitacao.Recusada == false)
            {
                ItemViagem itemViagem = new ItemViagem();
                itemViagem.IdViagem = solicitacao.IdViagem;
                itemViagem.IdUsuarioCarona = solicitacao.IdUsuario;

                _repositoryItemViagem.Save(itemViagem);
            }

            return NoContent();
        }
    }
}
