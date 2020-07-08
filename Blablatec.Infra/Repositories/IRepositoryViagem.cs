using Blablatec.Domain.Dto;
using Blablatec.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blablatec.Infra.Repositories
{
    public interface IRepositoryViagem: IRepository<Viagem>
    {
       public List<ViagemDtoSaida> ListasViagensEmAberto();
        public Task<List<ViagemDtoSaida>> ListarViagensOferecidas();
    }
}