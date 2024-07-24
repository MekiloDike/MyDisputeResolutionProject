using DisputeResolutionCore.Dto;
using DisputeResolutionCore.Enum;
using DisputeResolutionInfrastructure.Entity;
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
        Task<GenericResponse<DisputeResponseLogDto>> GetLoggedTransaction(string transactionReference);
        Task<(bool isCreated, string logcode)> StartInterswitchCall(DisputeResquestLog Request);

    }
}
