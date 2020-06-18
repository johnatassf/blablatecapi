using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blablatec.Domain.Dto;
using Blablatec.Domain.Model;
using Blablatec.Infra.Repositories;
using Blablatec.Infra.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blablatec.Controllers
{
    [ApiController]
    [Route("viagens")]
    public class ViagemController : Controller
    {
        private readonly IRepository<Viagem> _reposotoryViagem;
        private readonly IRepository<Usuario> _repositoryUser;
        private readonly IMapper _mapper;
        private readonly int _idUsuarioLogado;

        public ViagemController(
            IRepository<Viagem> reposotoryViagem,
            IRepository<Usuario> repositoryUser,
            IMapper mapper,
            IServiceInformationUser servicoInformacaoUsuario)
        {
            _reposotoryViagem = reposotoryViagem;
            _repositoryUser = repositoryUser;
            _mapper = mapper;
            _idUsuarioLogado = Convert.ToInt32(servicoInformacaoUsuario.IdUsuario);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var viagems = _reposotoryViagem.GetEntityByExpression(includes: v=> v.Motorista);

            return Ok(viagems);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var viagem = _reposotoryViagem.GetAll();

            if (viagem == null)
                return NotFound("Viagem {id} não encontrada");

            return Ok(viagem);
        }

        [HttpGet("motorista/{id}")]
        public IActionResult GetViagensPorMotorista([FromRoute] decimal id)
        {
            var motorista = _repositoryUser.GetOne(u => u.Id == id);

            if (motorista == null)
                return NotFound("Motorista não encontrado");

            var viagems = _reposotoryViagem.GetAll(v => v.IdMotorista == id);

            return Ok(viagems);
        }

        [HttpGet("ativas/motorista/{id}")]
        public IActionResult GetViagensAtivaPorMotorista([FromRoute] decimal id)
        {
            var motorista = _repositoryUser.GetOne(u => u.Id == id);

            if (motorista == null)
                return NotFound("Motorista não encontrado");

            var viagems = _reposotoryViagem.GetAll(v => v.IdMotorista == id);

            return Ok(viagems);
        }

        [HttpPost()]
        public IActionResult CriarViagem(ViagemDtoEntrada viagemEntrada)
        {
            var motorista = _repositoryUser.GetById(_idUsuarioLogado);

            if (motorista == null)
                return BadRequest("Motorista não encontrado");
            var viagem = _mapper.Map<Viagem>(viagemEntrada);
            viagem.IdMotorista = motorista.Id;
            viagem = _reposotoryViagem.Save(viagem);


            return Created(nameof(GetById), viagem);
        }


    }
}