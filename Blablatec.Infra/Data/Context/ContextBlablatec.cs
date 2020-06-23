using Blablatec.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Blablatec.Infra
{
    public class ContextBlablatec : DbContext
    {
        private readonly IConfiguration _configuration;

        public ContextBlablatec(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        DbSet<Usuario> Usuarios { get; set; }
        DbSet<ItemViagem> ItemViagems { get; set; }
        DbSet<Viagem> Viagens { get; set; }
        DbSet<Carro> Carros { get; set; }
        DbSet<SolicitacaoViagem> SolicitacaoViagem { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_configuration["ConnectionString:Connection"]);
        }
    }
}
