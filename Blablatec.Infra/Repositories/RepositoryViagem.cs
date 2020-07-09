using AutoMapper;
using Blablatec.Domain.Dto;
using Blablatec.Domain.Model;
using Blablatec.Infra.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blablatec.Infra.Repositories
{
    public class RepositoryViagem : BaseRepository<Viagem>, IRepository<Viagem>, IRepositoryViagem
    {
        private readonly IMapper _mapper;
        private readonly IRepository<SolicitacaoViagem> _repositorySolicitacaoViagem;
        private readonly IRepository<RotaAtiva> _repositoryRotaAtiva;
        private readonly int _idUsuario;

        public RepositoryViagem(ContextBlablatec contextBlablatec,
            IMapper mapper,
            IRepository<SolicitacaoViagem> repositorySolicitacaoViagem,
            IServiceInformationUser serviceInformationUser,
            IRepository<RotaAtiva> repositoryRotaAtiva) : base(contextBlablatec)
        {
            _mapper = mapper;
            _repositorySolicitacaoViagem = repositorySolicitacaoViagem;
            _idUsuario = Convert.ToInt32(serviceInformationUser.IdUsuario);
            _repositoryRotaAtiva = repositoryRotaAtiva;
        }



        public List<ViagemDtoSaida> ListasViagensEmAberto()
        {
            var viagens = GetEntityByExpression(v => v.Finalizacao == null
                && v.DataViagem > DateTime.Now
                && v.DataInicio == null
                && v.Cancelada == null, v => v.Motorista);

            var viagensDtoSaida = _mapper.Map<List<ViagemDtoSaida>>(viagens);
            var viagensSolicitadas = _repositorySolicitacaoViagem.GetAll();

            viagensDtoSaida.ForEach(viagem =>
           {
               viagem.MotoristaDaCorrida = viagem.IdMotorista == _idUsuario;
               viagem.JaSolicitado = viagensSolicitadas.Where(v => v.IdUsuario == _idUsuario
               && v.IdViagem == viagem.Id)
               .Any();
           });



            return viagensDtoSaida;
        }


        public async Task<List<ViagemDtoSaida>> ListarViagensOferecidas()
        {
            var viagens = GetEntityByExpression(v => v.Finalizacao == null && v.Cancelada == null
                        && v.DataViagem > DateTime.Now && v.IdMotorista == _idUsuario, v => v.Motorista);

            var idsViagens = viagens.Select(v => v.Id).ToList();
            var viagensDtoSaida = _mapper.Map<List<ViagemDtoSaida>>(viagens);
            var viagensSolicitadas = _repositorySolicitacaoViagem.GetAll(vs => idsViagens.Contains(vs.IdViagem));


            var rotaAtiva = await _repositoryRotaAtiva.GetOne(r => r.Viagem.IdMotorista == _idUsuario && r.Viagem.Finalizacao == null);

            viagensDtoSaida.ForEach(viagem =>
            {
                viagem.MotoristaDaCorrida = viagem.IdMotorista == _idUsuario;
                viagem.QuantidadeDeSolicitacaoAtiva = viagensSolicitadas.Where(v => v.IdViagem == viagem.Id && (v.Recusada == false || v.Recusada == null))
                .Count();
            });

            if (rotaAtiva != null)
                viagensDtoSaida.Where(v => rotaAtiva.IdViagem == v.Id).FirstOrDefault().EmAndamento = true;

            return viagensDtoSaida;
        }
    }
}
