using Blablatec.Domain.Model;
using Blablatec.Infra.Repositories;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
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

        public async Task<bool> Send(string emailDestinatario, string nomeDestinatario, string subject, object templateData)
        {
         
            var apiKey = _configuration["SendGrid:Key"];
            var client = new SendGridClient(apiKey);
            var message = new SendGridMessage()
            {
                From = new EmailAddress(_configuration["EmailBlablatec"], "Blablatec"),
                Personalizations = new List<Personalization>(),
                Subject = subject
            };
            //message.TemplateId = _configuration["TemplateEmailId"];

            message.Personalizations.Add(new Personalization()
            {
                TemplateData = templateData
            });

            message.AddTo(new EmailAddress(emailDestinatario, nomeDestinatario));

            var response = await client.SendEmailAsync(message);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                return true;
            else
                return false;
            
        }
    }
}
