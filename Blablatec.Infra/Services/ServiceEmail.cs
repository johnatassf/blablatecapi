using Blablatec.Domain.Model;
using Blablatec.Infra.Repositories;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Blablatec.Infra.Services
{
    public class ServiceEmail : IServiceEmail
    {
        private readonly IConfiguration _configuration;


        public ServiceEmail(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Send(string emailDestinatario, string nomeDestinatario, string assunto, string templateData, string plainTextContent)
        {
            try
            {
                var apiKey = _configuration["SendGrid:Key"];
                var client = new SendGridClient(apiKey);
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(_configuration["SendGrid:EmailBlablatec"], "Blablatec"),
                    Subject = assunto,
                    PlainTextContent = plainTextContent,
                    HtmlContent = templateData
                };
                msg.AddTo(new EmailAddress("johnatas.santos@fatec.sp.gov.br", "User"));
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.Accepted)
                    throw new Exception("Erro ao enviar e-mail de nova senha");

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao enviar e-mail de nova senha");
            }
        }
    }

}
