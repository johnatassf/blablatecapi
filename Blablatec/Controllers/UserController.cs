using Blablatec.Domain.Model;
using Blablatec.Infra;
using Blablatec.Infra.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blablatec.Controllers
{

    [ApiController]
    [Route("user")]
    public class UserController: ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly ContextBlablatec _contextBlablatec;

        public UserController(ILogger<UserController> logger, ContextBlablatec contextBlablatec)
        {
            _logger = logger;
            _contextBlablatec = contextBlablatec;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IRepository<Usuario> repositoryUser = new BaseRepository<Usuario>(_contextBlablatec);
            
            return Ok(repositoryUser.GetAll());
        }

    }
}
