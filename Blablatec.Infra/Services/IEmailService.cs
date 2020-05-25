using SendGrid;
using System.Threading.Tasks;

namespace Blablatec.Infra.Services
{
    public interface IServiceEmail
    {
        Task Send(string emailDestinatario, string nomeDestinatario, string assunto, string templateData, string plainTextContent);
    }
}