using Blablatec.Domain.Model;
using Blablatec.Infra;
using Blablatec.Infra.Repositories;
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
    [Route("carro")]
    public class CarroController : ControllerBase
    {
        private readonly ILogger<CarroController> _logger;
        private readonly IRepository<Carro> _repositoryCarro;
        private readonly ContextBlablatec _contextBlablatec;

        public CarroController(ILogger<CarroController> logger,
            IRepository<Carro> repositoryCarro)
        {
            _logger = logger;
            _repositoryCarro = repositoryCarro;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var carros = _repositoryCarro.GetAll();

            return Ok(carros);
        }

        [HttpGet("{id}")]
        public IActionResult GetId([FromRoute] int id)
        {
            var carros = _repositoryCarro.GetById(id);

            return Ok(carros);
        }

        [HttpPost()]
        public IActionResult CreateCarro(Carro carro)
        {
            if (carro == null)
                return BadRequest("Dados inválidos");

            //Fazer verificação do rm ou documento ja criado

            return Ok(_repositoryCarro.Save(carro));
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarCarro([FromRoute] int id, Carro carro)
        {
            if (carro.Id != id)
                return StatusCode(StatusCodes.Status409Conflict,
                  $"Id do carro divergente do id informado");

            var user = _repositoryCarro.Update(carro);

            return Ok(user);
        }
    }
}
