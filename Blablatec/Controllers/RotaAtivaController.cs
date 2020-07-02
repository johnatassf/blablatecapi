using AutoMapper;
using Blablatec.Domain.Dto;
using Blablatec.Domain.Model;
using Blablatec.Infra.Repositories;
using Blablatec.Infra.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blablatec.Controllers
{
    [Route("rotas")]
    public class RotaAtivaController: ControllerBase
    {
        private readonly IRepository<RotaAtiva> _repositoryRotaAtiva;
        private readonly int _idUser;
        private readonly IMapper _mapper;
        private readonly IRepository<Viagem> _repositoryViagem;
        public RotaAtivaController(
         IRepository<RotaAtiva> repositoryRotaAtiva,
         IServiceInformationUser servicoInformacaoUsuario,
         IRepository<Viagem> repositoryViagem,
        IMapper mapper)
        {
            _repositoryRotaAtiva = repositoryRotaAtiva;
            _idUser = Convert.ToInt32(servicoInformacaoUsuario.IdUsuario);
            _mapper = mapper;
            _repositoryViagem = repositoryViagem;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var viagems = _repositoryRotaAtiva.GetAll();

            return Ok(viagems);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var viagem = _repositoryRotaAtiva.GetAll();

            if (viagem == null)
                return NotFound("Viagem {id} não encontrada");

            return Ok(viagem);
        }

        [HttpGet("ativa")]
        public IActionResult GetRotasAtivasPorUsuarioLogado()
        {
            var rotaAtiva =  _repositoryRotaAtiva.GetEntityByExpression(r =>
                r.Viagem.IdMotorista == _idUser 
             || r.Viagem.ItensViagens.Where(v=> v.IdUsuarioCarona == _idUser).Any()
             && r.Viagem.Finalizacao == null, it => it.Viagem.ItensViagens)
                .FirstOrDefault();

            var rotaAtivaMapped = _mapper.Map<RotaAtivaDtoSaida>(rotaAtiva);
            rotaAtivaMapped.IdUsuarioLogado = _idUser;
            rotaAtivaMapped.IsMotorista = _idUser == rotaAtivaMapped.idMotorista;
          
            if (rotaAtiva == null)
                return BadRequest("Nenhuma rota ativa encontrada");

            return Ok(rotaAtivaMapped);
        }

        [HttpPost("ativa/{id}")]
        public async Task<IActionResult>CriarRotaEmAndamento([FromBody] RotaAtivaDtoEntrada rota, int id)
        {
            var viagem = _repositoryViagem.GetById(id);

            if (viagem == null)
                return NotFound("Viagem não encontrada");

            if (viagem.Finalizacao != null)
                return BadRequest($"Viagem {viagem.Id} já foi finalizada");

            var rotasEmAndamento = await _repositoryRotaAtiva.GetOne(r => r.IdViagem == viagem.Id);

            if (rotasEmAndamento != null)
                return BadRequest($"Viagem {viagem.Id} já obtem\\obteve uma rota em andamento");

            if (viagem.IdMotorista != _idUser)
                return BadRequest("Motorista logado não condiz com a viagem selecionada");

            var rotaAtiva = new RotaAtiva
            {
                IdViagem = viagem.Id,
                LatitudeAtual = rota.LatitudeAtual,
                LongitudeAtual = rota.LongitudeAtual
            };

            rotaAtiva = _repositoryRotaAtiva.Save(rotaAtiva);

            return Created(nameof(GetById), viagem);
        }

        [HttpDelete("ativa/{id}")]
        public IActionResult FinalizarCorridaEmAndamento(int id)
        {
            var rotaAtiva = _repositoryRotaAtiva.GetEntityByExpression(r =>
               r.Viagem.IdMotorista == _idUser
            || r.Viagem.ItensViagens.Where(v => v.IdUsuarioCarona == _idUser).Any()
            && r.Viagem.Finalizacao == null
            && r.Viagem.Id == id, r => r.Viagem, it => it.Viagem.ItensViagens).FirstOrDefault();


            if (rotaAtiva.Viagem != null)
            {
                rotaAtiva.Viagem.Finalizacao = DateTime.Now;
                _repositoryViagem.Update(rotaAtiva.Viagem);
            }
                

            return NoContent();
        }
    }
}
