using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using AutoMapper;
using Blablatec.Infra;
using Blablatec.Infra.Authorize;
using Blablatec.Infra.Repositories;
using Blablatec.Infra.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Blablatec
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
           

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blablatec api", Version = "v1" });

                var caminhoAplicacao = AppDomain.CurrentDomain.BaseDirectory;

                foreach (var nomeArquivo in Directory.GetFiles(caminhoAplicacao, "*.xml", SearchOption.AllDirectories))
                    c.IncludeXmlComments(nomeArquivo);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "Bearer "
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                  });

            });


            var key = Encoding.ASCII.GetBytes(Configuration["Security:Key"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<ContextBlablatec>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies().ToArray());
            services.AddOptions();
            services.AddScoped<IConfiguration>(c => Configuration);

        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assemblies = ObterAssemblies();

            foreach (var assembly in assemblies)
            {
                builder.RegisterAssemblyTypes(assembly)
                  .Where(t => t.Name.StartsWith("Repository")
                  || t.Name.StartsWith("Service"))
                  .AsImplementedInterfaces();
            }
               
            // Register your own things
            // Register your own things directly with Autofac, like:
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IRepository<>));
            builder.RegisterType<JwtIdentityAuthentication>().As<IAuthentication>();
            builder.RegisterType<RepositoryUserManage>().As<IRepositoryUserManage>();
            builder.RegisterType<ServiceEmail>().As<IServiceEmail>();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();
            builder.RegisterType<ServiceInformationUser>().As<IServiceInformationUser>();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder => {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

          
                

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseSwagger(c => { c.RouteTemplate = "api-docs/{documentName}/swagger.json"; });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api-docs/v1/swagger.json", "My API V1");
                c.RoutePrefix = "api-docs";



            });
            app.UseSwagger();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private static List<Assembly> ObterAssemblies()
        {   
            var caminhoAplicacao = AppDomain.CurrentDomain.BaseDirectory;
            var caminhosAssemblies = new[]
            {
                Path.Combine(caminhoAplicacao, "Blablatec.Domain.dll"),
                Path.Combine(caminhoAplicacao, "Blablatec.Infra.dll")
            };

            return caminhosAssemblies.Select(Assembly.LoadFile).ToList();
        }

    }
}
