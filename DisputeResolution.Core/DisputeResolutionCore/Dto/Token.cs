using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeResolutionCore.Dto
{
    
    public class TokenResponse
    {
        public string? access_token { get; set; }
        public string? token_type { get; set; }
        public int expires_in { get; set; }
        public string? scope { get; set; }
        public string? core_id { get; set; }
        public string? client_authorization_domain { get; set; }
        public string? domain_code { get; set; }
        public string? env { get; set; }
        public string? client_name { get; set; }
        public string? jti { get; set; }
    }

}
