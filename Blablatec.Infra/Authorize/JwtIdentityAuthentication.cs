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
        private readonly SigningConfiguration _signingConfiguration;

        public JwtIdentityAuthentication(SigningConfiguration signingConfiguration)
        {
            _signingConfiguration = signingConfiguration;
        }

        public AuthenticationResult Authenticate(IUser user)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Ra),
            new Claim("Data", ToJson(user))
        };

            var identity = new ClaimsIdentity(new GenericIdentity(user.Ra, "Login"), claims);

            var created = DateTime.UtcNow;
            var expiration = created + TimeSpan.FromSeconds(60000);
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = "FSL",
                Audience = "FSL",
                SigningCredentials = _signingConfiguration.SigningCredentials,
                Subject = identity,
                NotBefore = created,
                Expires = expiration
            });

            var result = new AuthenticationResult
            {
                Success = true,
                Authenticated = true,
                Created = created.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = expiration.ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = handler.WriteToken(securityToken),
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
