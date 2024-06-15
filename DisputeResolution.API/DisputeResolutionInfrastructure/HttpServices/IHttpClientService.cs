using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeResolutionInfrastructure.HttpServices
{
    public interface IHttpClientService
    {
        Task<T> GetTransaction<T>(string urlWithParams, string token, string domain) where T : class;

         Task<T> GetToken<T>();
    }
}
