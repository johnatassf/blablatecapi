using Blablatec.Domain.Dto;
using Blablatec.Domain.Model;
using System.Collections.Generic;

namespace Blablatec.Infra.Repositories
{
    public interface IRepositoryViagem: IRepository<Viagem>
    {
        List<ViagemDtoSaida> ListasViagensEmAberto();
    }
}