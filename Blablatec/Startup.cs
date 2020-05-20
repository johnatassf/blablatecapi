using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using Blablatec.Infra;
using Blablatec.Infra.Authorize;
using Blablatec.Infra.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Blablatec
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public Startup(IHostingEnvironment env)
        //{
        //    // In ASP.NET Core 3.0 `env` will be an IWebHostEnvironment, not IHostingEnvironment.
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        //        .AddEnvironmentVariables();
        //    this.Configuration = builder.Build();
        //}
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

            });

            var siginingConfiguration = new SigningConfiguration();
            services.AddSingleton(siginingConfiguration);


            var token = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(Configuration.GetSection(typeof(TokenConfiguration).Name)).Configure(token);
            services.AddSingleton(token);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters.IssuerSigningKey = siginingConfiguration.Key;
                opt.TokenValidationParameters.ValidAudience = token.ValidAudience; // dynamic
                opt.TokenValidationParameters.ValidIssuer = token.ValidIssuer;  // dynamic
                opt.TokenValidationParameters.ValidateIssuerSigningKey = token.ValidateIssuerSigningKey;  // dynamic
                opt.TokenValidationParameters.ValidateLifetime = token.ValidateLifetime;  // dynamic
                opt.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(TokenConfiguration.Policy, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });


            services.AddScoped<ContextBlablatec>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies().ToArray());

            services.AddOptions();

        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {

            var dataAccess = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(dataAccess)
                   .Where(t => t.Name.StartsWith("Repository")
                   || t.Name.StartsWith("Service"))
                   .AsImplementedInterfaces();
            // Register your own things directly with Autofac, like:
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IRepository<>));
            builder.RegisterType<JwtIdentityAuthentication>().As<IAuthentication>();
            builder.RegisterType<RepositoryUserManage>().As<IRepositoryUserManage>();
        }





        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger(c => { c.RouteTemplate = "api-docs/{documentName}/swagger.json"; });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api-docs/v1/swagger.json", "My API V1");
                c.RoutePrefix = "api-docs";
            });

            app.UseSwagger();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }


    }
}
