

using Blablatec.Domain.Model;

namespace Blablatec.Infra.Repositories
{
    public class IRepositoryItemViagem<T>: BaseRepository<ItemViagem>, IRepository<ItemViagem> {

        public IRepositoryItemViagem(ContextBlablatec contexto): base(contexto)
        {

        }

        public override ItemViagem Save(ItemViagem itemViagem)
        {
            //Fazer validações da viagem 
            return itemViagem;
        }
    }
}
