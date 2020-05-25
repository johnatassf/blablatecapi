using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blablatec.Infra.Services
{
    public class ServiceInformationUser : IServiceInformationUser
    {
        public ServiceInformationUser(IHttpContextAccessor contextoHttp)
        {
            if (contextoHttp == null)
                throw new ArgumentNullException(nameof(contextoHttp));

            var contextoAtual = contextoHttp.HttpContext;
            if (contextoAtual == null || !contextoAtual.User.Identity.IsAuthenticated)
                return;

            IdUsuario = contextoAtual
                .User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            Ra = contextoAtual.User
                .Claims.FirstOrDefault(c => c.Type == "Ra")?.Value;

            Nome = contextoAtual
                .User.Claims.FirstOrDefault(c => c.Type == "Nome")?.Value;

          

            Token = ObterTokenRequisicao(contextoAtual);
        }

        public string IdUsuario { get; set; }
        public string Ra { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Token { get; set; }

        private string ObterTokenRequisicao(HttpContext contextoAtual)
        {
            string autorizacao = contextoAtual.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(autorizacao))
                return null;

            return autorizacao.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                ? autorizacao.Substring("Bearer ".Length).Trim()
                : null;
        }
    }
}
