using AutoMapper;
using Blablatec.Domain.Dto;
using Blablatec.Domain.Interface;
using Blablatec.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Infra.Mapeamento
{
    public class Mapeamento : Profile
    {
        public Mapeamento()
        {
            CreateMap<RegistroUsuarioDto, Usuario>();
            CreateMap<Usuario, RegistroUsuarioDto>();

            CreateMap<UpdateProfile, Usuario>()
                .ForMember(dest => dest.Nome, config => config.MapFrom(source => source.Name))
                 .ForMember(dest => dest.Sobrenome, config => config.MapFrom(source => source.LastName));

            CreateMap<ViagemDtoEntrada, Viagem>()
            .ForMember(dest => dest.DataViagem, config => config.MapFrom(source => source.Viagem));

            CreateMap<Viagem, ViagemDtoSaida>();

            CreateMap<RotaAtiva, RotaAtivaDtoSaida>()
                .ForMember(dest => dest.Id, config => config.MapFrom(source => source.Id))
                .ForMember(dest => dest.LatitudeAtual, config => config.MapFrom(source => source.LatitudeAtual))
                .ForMember(dest => dest.LongitudeAtual, config => config.MapFrom(source => source.LongitudeAtual))
                .ForMember(dest => dest.idMotorista, config => config.MapFrom(source => source.Viagem.IdMotorista))
                .ForMember(dest => dest.PontoFinal, config => config.MapFrom(source => source.Viagem.PontoFinal));

        }
    }
}
