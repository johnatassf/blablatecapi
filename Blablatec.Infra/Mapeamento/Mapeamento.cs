using AutoMapper;
using Blablatec.Domain.Dto;
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
        }
    }
}
