using Blablatec.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Infra.Repositories
{
    public class IRepositorioSolicitacaoViagem<T> : BaseRepository<solicitacaoViagem>, IRepository<solicitacaoViagem>
    {

        public IRepositorioSolicitacaoViagem(ContextBlablatec contexto) : base(contexto)
        {

        }

        public solicitacaoViagem Save(solicitacaoViagem itemViagem)
        {
            //Fazer validações da viagem 
            return itemViagem;
        }

       


    }
}