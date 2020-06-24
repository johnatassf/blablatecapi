using AutoMapper;
using Blablatec.Domain.Dto;
using Blablatec.Domain.Model;
using Blablatec.Infra.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blablatec.Infra.Repositories
{
    public class RepositoryViagem : BaseRepository<Viagem>, IRepository<Viagem>, IRepositoryViagem
    {
        private readonly IMapper _mapper;
        private readonly IRepository<solicitacaoViagem> _repositorySolicitacaoViagem;
        private readonly int _idUsuario;
        private readonly IServiceInformationUser _serviceInformationUser;

        public RepositoryViagem(ContextBlablatec contextBlablatec,
            IMapper mapper,
            IRepository<solicitacaoViagem> repositorySolicitacaoViagem,
            IServiceInformationUser serviceInformationUser) : base(contextBlablatec)
        {
            _mapper = mapper;
            _repositorySolicitacaoViagem = repositorySolicitacaoViagem;
            _idUsuario = Convert.ToInt32(serviceInformationUser.IdUsuario);
        }



        public List<ViagemDtoSaida> ListasViagensEmAberto()
        {
            var viagens  = GetEntityByExpression(v => v.Finalizacao == null
            && v.DataViagem < DateTime.Now, v => v.Motorista);

           var viagensDtoSaida = _mapper.Map<List<ViagemDtoSaida>>(viagens);
            var viagensSolicitadas = _repositorySolicitacaoViagem.GetAll();

            viagensDtoSaida.ForEach(viagem =>
           {
               viagem.MotoristaDaCorrida = viagem.IdMotorista == _idUsuario;
               viagem.JaSolicitado = viagensSolicitadas.Where(v => v.IdUsuario== _idUsuario
               && v.IdViagem == viagem.Id)
               .Any();
           });

            return viagensDtoSaida;
        } 
    }
}
