using DisputeResolutionCore.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DisputeResolutionCore.Dto.CreateDisputeResponse;

namespace DisputeResolutionCore.Interface
{
    public interface IDispute
    {
        Task CreateDispute(CreateDisputeRequest request);
        Task<GetDisputeResponse> GetDispute(string logCode);
    }
}
