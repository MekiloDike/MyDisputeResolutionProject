using DisputeResolutionCore.Dto;
using DisputeResolutionInfrastructure.HttpServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeResolutionCore.Utility
{
    public class TokenService
    { 
        private readonly IHttpClientService _httpClientService;
        public TokenService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;            
        }
        public async Task<TokenResponse> GetAccessToken()
        {
            var result = await _httpClientService.GetToken<TokenResponse>();
            return result;

        }
    }

}
