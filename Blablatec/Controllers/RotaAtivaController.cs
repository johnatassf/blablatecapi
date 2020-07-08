using AutoMapper;
using Blablatec.Domain.Dto;
using Blablatec.Domain.Model;
using Blablatec.Infra.Authorize;
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
    public class RotaAtivaController : ControllerBase
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
            var rotaAtiva = _repositoryRotaAtiva.GetEntityByExpression(r =>
               (r.Viagem.IdMotorista == _idUser || r.Viagem.ItensViagens.Where(v => v.IdUsuarioCarona == _idUser).Any())
            && r.Viagem.Finalizacao == null, it => it.Viagem.ItensViagens)
                .FirstOrDefault();

            if (rotaAtiva == null)
                return Ok(new BaseResult<object>() { Message = "Nenhuma rota ativa encontrada", Success = false });

            var rotaAtivaMapped = _mapper.Map<RotaAtivaDtoSaida>(rotaAtiva);
            rotaAtivaMapped.IdUsuarioLogado = _idUser;
            rotaAtivaMapped.IsMotorista = _idUser == rotaAtivaMapped.idMotorista;

            return Ok(new BaseResult<RotaAtivaDtoSaida>() { Message = "Nenhuma rota ativa encontrada", Success = true, Data = rotaAtivaMapped });
        }

        [HttpPost("ativa/{idViagem}")]
        public async Task<IActionResult> CriarRotaEmAndamento(int idViagem)
        {
            var viagem = _repositoryViagem.GetById(idViagem);

            if (viagem == null)
                return NotFound("Viagem não encontrada");

            if (viagem.Finalizacao != null)
                return BadRequest($"Viagem informada já finalizada");

            var rotaEmAndamento = await _repositoryRotaAtiva.GetOne(r => r.IdViagem == viagem.Id);

            if (rotaEmAndamento != null)
                return BadRequest($"Está viagem ja possui uma rota em andamento");

            if (viagem.IdMotorista != _idUser)
                return BadRequest("Motorista logado não condiz com a viagem selecionada");

            if (viagem.IdMotorista != _idUser)
                return BadRequest("Motorista logado não condiz com a viagem selecionada");

            var existRodaEmAndamento = _repositoryRotaAtiva.GetEntityByExpression(r => r.Viagem.IdMotorista == _idUser
            && r.Viagem.Finalizacao == null,
            v => v.Viagem).Any();

            if (existRodaEmAndamento)
                return BadRequest("Motorista ja possui um rota em andamento");

            return Created(nameof(GetById), _repositoryRotaAtiva.Save(new RotaAtiva() { IdViagem = viagem.Id }));
        }

        [HttpPut("ativa/{idViagem}")]
        public async Task<IActionResult> AtualizarRotaEmAndamento([FromBody] RotaAtivaDtoEntrada rota, int idViagem)
        {
            var viagem = _repositoryViagem.GetById(idViagem);

            if (viagem == null)
                return NotFound("Viagem não encontrada");

            if (viagem.Finalizacao != null)
                return BadRequest($"Viagem {viagem.Id} já foi finalizada");

            if (viagem.IdMotorista != _idUser)
                return BadRequest("Motorista logado não condiz com a viagem selecionada");

            var rotaEmAndamento = await _repositoryRotaAtiva.GetOne(r => r.IdViagem == viagem.Id && rota.Id == r.Id);

            rotaEmAndamento.LatLng = rota.LatLng;

            _repositoryRotaAtiva.Save(rotaEmAndamento);

            return NoContent();
        }


        [HttpPost("ativa/{idViagem}/finalizar")]
        public async Task<IActionResult> FinalizarCorridaEmAndamento([FromBody] RotaAtivaDtoEntrada rota, int idViagem)
        {
            var viagem = _repositoryViagem.GetById(idViagem);

            if (viagem == null)
                return NotFound("Viagem não encontrada");

            if (viagem.Finalizacao != null)
                return BadRequest($"Viagem {viagem.Id} já foi finalizada");

            if (viagem.IdMotorista != _idUser)
                return BadRequest("Motorista logado não condiz com a viagem selecionada");

            var rotaEmAndamento = await _repositoryRotaAtiva.GetOne(r => r.IdViagem == viagem.Id && rota.Id == r.Id);

            rotaEmAndamento.LatLng = rota.LatLng;

            _repositoryRotaAtiva.Update(rotaEmAndamento);


            viagem.Finalizacao = DateTime.Now;
            _repositoryViagem.Update(viagem);


            return NoContent();
        }
    }
}
