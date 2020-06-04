using Blablatec.Domain.Model;
using Blablatec.Infra.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blablatec.Controllers
{
    [ApiController]
    [Route("viagem")]
    public class ViagemController : Controller
    {
        private readonly IRepository<Viagem> _repositoryViagem;
        private readonly IRepository<Usuario> _repositoryUser;

        public ViagemController(
            IRepository<Viagem> repositoryViagem,
            IRepository<Usuario> repositoryUser)
        {
            _repositoryViagem = repositoryViagem;
            _repositoryUser = repositoryUser;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var viagems = _repositoryViagem.GetAll();

            viagems.ForEach(p => p.Motorista = _repositoryUser.GetById(p.IdMotorista));

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

        [HttpPost()]
        public async Task<IActionResult> CriarViagem(Viagem viagem)
        {
            var motorista = await _repositoryUser.GetOne(u => u.Id == viagem.Id);

            if (motorista == null)
                return BadRequest("Motorista não encontrado");

            viagem = _repositoryViagem.Save(viagem);

            return Created(nameof(GetById), viagem);
        }
    }
}
