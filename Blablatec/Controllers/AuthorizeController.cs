using Blablatec.Infra.Authorize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blablatec.Controllers
{
    [ApiController]
    [Route("login")]
    public class AuthorizeController: ControllerBase
    {
        private readonly IAuthentication _serviceAuthentication;
        //private readonly IServiceAuthorization _serviceAuthorization;

        public AuthorizeController(IAuthentication serviceAuthentication)
        {
            _serviceAuthentication = serviceAuthentication;
            //_serviceAuthorization = serviceAuthorization;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody] LoginUser loginUser)
        {
            //var authorization = _serviceAuthorization.Authorize(loginUser);
            //if (!authorization.Success)
            //{
            //    return Ok(authorization);
            //}

            var authentication = _serviceAuthentication.Authenticate(loginUser);
            if (!authentication.Success)
            {
                return Ok(authentication);
            }

            return Ok(authentication);
        }
    }
}
