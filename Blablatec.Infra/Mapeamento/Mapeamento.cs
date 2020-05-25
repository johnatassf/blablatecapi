using AutoMapper;
using Blablatec.Domain.Dto;
using Blablatec.Domain.Interface;
using Blablatec.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Infra.Mapeamento
{
    public class Mapeamento: Profile
    {
        public Mapeamento()
        {
            CreateMap<RegistroUsuarioDto, Usuario>();
            CreateMap<Usuario, RegistroUsuarioDto>();

            CreateMap<UpdateProfile, Usuario>()
                .ForMember(dest => dest.Nome, config => config.MapFrom(source => source.Name))
                 .ForMember(dest => dest.Sobrenome, config => config.MapFrom(source => source.LastName));


        }
    }
}
