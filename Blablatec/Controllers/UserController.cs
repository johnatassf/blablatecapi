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
        private readonly IRepository<Usuario> _repositoryUser;
        private readonly ContextBlablatec _contextBlablatec;

        public UserController(ILogger<UserController> logger, 
            IRepository<Usuario> repositoryUser)
        {
            _logger = logger;
            _repositoryUser = repositoryUser;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var usuarios =_repositoryUser.GetAll();
            
            return Ok(usuarios);
        }

    }
}
