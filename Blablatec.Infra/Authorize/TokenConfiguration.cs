using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blablatec.Infra.Authorize
{
    public class TokenConfiguration
    {
        public const string Policy = "Bearer";
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public int ExpirationInSeconds { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
    }
}
