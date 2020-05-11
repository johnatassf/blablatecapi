using Blablatec.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blablatec.Controllers
{
    [ApiController]
    [Route("motoristas-cadastrados")]
    public class RegisteredDriversController : ControllerBase
    {
        private readonly ILogger<RegisteredDriversController> _logger;

        public RegisteredDriversController(ILogger<RegisteredDriversController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var motoristasCadastrados = new Drivers().ObterMotoristasCadastrados();

            return Ok(motoristasCadastrados);
        }

     
    }
}
