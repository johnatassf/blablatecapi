using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blablatec.Infra.Data.Context
{
    public static class SettingEntity
    {
        public static void ConfigureServices(IServiceCollection service, IConfiguration configuration)
        {
            service.AddEntityFrameworkMySql()
                .AddDbContext<ContextBlablatec>(opcoes => opcoes.UseMySql(configuration.GetConnectionString("ConnectionString")));
        }
    }
}
