using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blablatec.Domain.Model;
using Blablatec.Infra.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blablatec.Controllers
{
    [ApiController]
    [Route("viagens")]
    public class ItemViagemController : Controller
    {
        private readonly IRepository<ItemViagem> _repositoryViagem;
        private readonly IRepository<Usuario> _repositoryUser;

        public ItemViagemController(
            IRepository<ItemViagem> repositoryViagem,
            IRepository<Usuario> repositoryUser)
        {
            _repositoryViagem = repositoryViagem;
            _repositoryUser = repositoryUser;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var viagems = _repositoryViagem.GetAll();

            return Ok(viagems);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var viagem = _repositoryViagem.GetAll();

            if (viagem == null)
                return NotFound("Viagem {id} não encontrada");

            return Ok(viagem);
        }

        [HttpGet("motorista/{id}")]
        public IActionResult GetViagensPorMotorista([FromRoute] decimal id )
        {
            var motorista = _repositoryUser.GetOne(u => u.Id == id);

            if (motorista == null)
                return NotFound("Motorista não encontrado");

            var viagems = _repositoryViagem.GetAll(v => v.IdMotorista == id);

            return Ok(viagems);
        }

        [HttpGet("ativas/motorista/{id}")]
        public IActionResult GetViagensAtivaPorMotorista([FromRoute] decimal id)
        {
            var motorista = _repositoryUser.GetOne(u => u.Id == id);

            if (motorista == null)
                return NotFound("Motorista não encontrado");

            var viagems = _repositoryViagem.GetAll(v => v.IdMotorista == id && v.Finalizacao == null);

            return Ok(viagems);
        }

        [HttpPost()]
        public async Task<IActionResult> CriarViagem(ItemViagem viagem)
        {
            var motorista = await _repositoryUser.GetOne(u => u.Id == viagem.IdMotorista);

            if (motorista == null)
                return BadRequest("Motorista não encontrado");

            viagem = _repositoryViagem.Save(viagem);

            return Created(nameof(GetById), viagem);
        }

        
    }
}