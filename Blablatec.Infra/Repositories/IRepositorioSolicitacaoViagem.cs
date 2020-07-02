using Blablatec.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Infra.Repositories
{
    public class IRepositorioSolicitacaoViagem<T> : BaseRepository<SolicitacaoViagem>, IRepository<SolicitacaoViagem>
    {

        public IRepositorioSolicitacaoViagem(ContextBlablatec contexto) : base(contexto)
        {

        }

        public SolicitacaoViagem Save(SolicitacaoViagem itemViagem)
        {
            //Fazer validações da viagem 
            return itemViagem;
        }

       


    }
}