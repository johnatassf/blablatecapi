using Blablatec.Domain.Model;
using Blablatec.Infra.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blablatec.Controllers
{
    public class EvaluationDriverController : ControllerBase
    {
        private readonly ILogger<EvaluationDriverController> _logger;
        private readonly IRepository<Usuario> _repositoryUser;
        private readonly IRepository<Evaluation> _repositoryEvaluation;

        public EvaluationDriverController(ILogger<EvaluationDriverController> logger,
            IRepository<Evaluation> repositoryEvaluation,
             IRepository<Usuario> repositoryUser)
        {
            _logger = logger;
            _repositoryUser = repositoryUser;
            _repositoryEvaluation = repositoryEvaluation;
        }


        [HttpPost()]
        public IActionResult CreateUser(Evaluation evaluation)
        {
            //Id usuario do token logado 

            if (evaluation == null)
                return BadRequest("Dados inválidos");

            if (_repositoryUser.Exists(evaluation.IdAvaliado))
                return BadRequest($"Id: {evaluation.IdAvaliado} do usuário avaliado não encontrado");

            if (_repositoryUser.Exists(evaluation.IdAvaliador))
                return BadRequest($"Id: {evaluation.IdAvaliador} do usuário avaliador não encontrado");

            evaluation = _repositoryEvaluation.Save(evaluation);

            //Fazer verificação do rm ou documento ja criado

            return Ok(evaluation);
        }


    }
}
