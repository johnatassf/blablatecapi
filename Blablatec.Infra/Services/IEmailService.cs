using System.Threading.Tasks;

namespace Blablatec.Infra.Services
{
    public interface IServiceEmail
    {
        Task<bool> Send(string emailDestinatario, string nomeDestinatario, string subject, object templateData);
    }
}