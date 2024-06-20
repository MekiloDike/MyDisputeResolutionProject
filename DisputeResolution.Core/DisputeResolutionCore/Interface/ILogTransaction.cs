using DisputeResolutionCore.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeResolutionCore.Interface
{
    public interface ILogTransaction
    {
        Task<GenericResponse<bool>> CreateTransactionLog(DisputeRequestLogDto Request);

        Task<GenericResponse<DisputeResponseLogDto>> GetLoggedTransaction(string transactionReference);`
    }
}
