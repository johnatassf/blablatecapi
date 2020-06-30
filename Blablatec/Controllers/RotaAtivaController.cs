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
        private readonly IRepository<ItemViagem> _repositoryItemViagem;
        public RotaAtivaController(
         IRepository<RotaAtiva> repositoryRotaAtiva,
         IServiceInformationUser servicoInformacaoUsuario)
        {
            _repositoryRotaAtiva = repositoryRotaAtiva;
            _idUser = Convert.ToInt32(servicoInformacaoUsuario.IdUsuario);
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

        [HttpGet("andamento")]
        public async Task<IActionResult> GetRotasAtivasPorUsuarioLogado()
        {
            //var viagem = await _repositoryRotaAtiva.GetEntityByExpression(r => r.ItemViagem.IdMotorista == _idUser || r.);

            //if (viagem == null)
            //    return NotFound("Nenhuma viagem ativa para este morista foi encotrada");

            return Ok();
        }

        [HttpPost("viagem/{id}")]
        public async Task<IActionResult> CriarViagem([FromBody] RotaAtivaDtoEntrada rota, [FromQuery] int id)
        {
            var viagem = _repositoryItemViagem.GetById(id);

            if (viagem == null)
                return NotFound("Viagem não encontrada");

            if(viagem.Finalizacao != null)
                return BadRequest($"Viagem {viagem.Id} já foi finalizada");

            var rotasEmAndamento = _repositoryRotaAtiva.GetOne(r => r.IdItemViagem == viagem.Id );
            
            if(rotasEmAndamento != null)
                return BadRequest($"Viagem {viagem.Id} já obtem uma rota em andamento");

            if (viagem.IdMotorista == _idUser)
                return BadRequest("Motorista logado não condiz com a viagem selecionada");
            
            var rotaAtiva = new RotaAtiva
            {
                IdItemViagem = viagem.Id,
                LatitudeAtual = rota.LatitudeAtual,
                LongitudeAtual = rota.LongitudeAtual
            };
           

            rotaAtiva = _repositoryRotaAtiva.Save(rotaAtiva);
          
            return Created(nameof(GetById), viagem);
        }
    }
}
