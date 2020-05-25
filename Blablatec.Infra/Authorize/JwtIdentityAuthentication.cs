using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Blablatec.Infra.Authorize
{
    public class JwtIdentityAuthentication : IAuthentication
    {
        private readonly IConfiguration _configuration;

        public JwtIdentityAuthentication(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthenticationResult Authenticate(IUser user)
        {


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Security:Key"]);


            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Ra),
            new Claim("Data", ToJson(user))
        };

            var notBefore = DateTime.UtcNow;
            var expires = DateTime.UtcNow + TimeSpan.FromSeconds(60000);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Ra.ToString()),
                    new Claim(ClaimTypes.Role, "Login")
                }),
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
