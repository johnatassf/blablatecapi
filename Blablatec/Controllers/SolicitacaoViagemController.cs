﻿using Blablatec.Domain.Model;
using Blablatec.Infra.Repositories;
using Blablatec.Infra.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blablatec.Controllers
{

    [ApiController]
    [Route("solicitacao-viagem")]
    public class SolicitacaoViagemController : ControllerBase
    {
        private readonly ILogger<SolicitacaoViagemController> _logger;
        private readonly IRepository<solicitacaoViagem> _repositorySolicitacaoViagem;
        private readonly IRepository<Viagem> _repositoryViagem;
        private readonly int _idUsuarioLogado;

        public SolicitacaoViagemController(ILogger<SolicitacaoViagemController> logger,
            IRepository<solicitacaoViagem> repositorySolicitacaoViagem,
            IRepository<Viagem> repositoryViagem,
            IServiceInformationUser serviceInformationUser)
        {
            _logger = logger;
            _repositorySolicitacaoViagem = repositorySolicitacaoViagem;
            _repositoryViagem = repositoryViagem;
            _idUsuarioLogado = Convert.ToInt32(serviceInformationUser.IdUsuario);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var carros = _repositorySolicitacaoViagem.GetAll();

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

            var solicitacaoViagem = new solicitacaoViagem()
            {
                IdUsuarioCarona = _idUsuarioLogado,
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

            var solicitacao = await _repositorySolicitacaoViagem.GetOne(s => s.IdUsuarioCarona == _idUsuarioLogado && s.IdViagem == idViagem);

            if (solicitacao == null)
                return BadRequest("Solicitacao viagem não encontrada");

            _repositorySolicitacaoViagem.Remove(solicitacao);
            
            return NoContent();
        }
    }
}