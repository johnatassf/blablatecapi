using Blablatec.Domain.Interface;
using Blablatec.Domain.Model;
using Blablatec.Infra.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Blablatec.Infra.Authorize
{
    public class JwtIdentityAuthentication : IAuthentication
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<Carro> _repositoryCarro;
        private readonly ContextBlablatec _contextBlablatec;

        public JwtIdentityAuthentication(
            IConfiguration configuration,
            IRepository<Carro> repositoryCarro,
             ContextBlablatec contextBlablatec)
        {
            _configuration = configuration;
            _repositoryCarro = repositoryCarro;
            _contextBlablatec = contextBlablatec;
        }

        public AuthenticationResult Authenticate(IUser user)
        {
            var carroMotorista = _repositoryCarro.GetAll(c => c.IdMotorista == user.Id);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Security:Key"]);


            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Ra),
            new Claim("Data", ToJson(user)),
            new Claim(ClaimTypes.Name, user.Ra.ToString()),
            new Claim("Ra", user.Ra.ToString()),
            new Claim("Id", user.Id.ToString()),
        };

            if (carroMotorista != null)
                claims.Add(new Claim(ClaimTypes.Role, "Motorista"));

            var notBefore = DateTime.UtcNow;
            var expires = DateTime.UtcNow + TimeSpan.FromSeconds(60000);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = notBefore,
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var result = new AuthenticationResult
            {
                Success = true,
                Authenticated = true,
                Created = notBefore.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = expires.ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = tokenHandler.WriteToken(token),
                Message = "OK"
            };

            return result;
        }

        private string ToJson<T>(
            T obj)
        {
            if (obj == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(obj,
                 new JsonSerializerSettings()
                 {
                     NullValueHandling = NullValueHandling.Ignore
                 });
        }
    }
}
