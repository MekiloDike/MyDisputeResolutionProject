using DisputeResolutionCore.Dto;

namespace DisputeResolutionCore.Interface
{
    public interface ITransaction
    { 
        Task<IpgTransactionResponse> GetIpgTransaction(IpgTransactionRequest request);
        Task<AgencyBankingResponse> GetAgencyBanking(AgencyBankingRequest request);        
        Task<TransferTransactionResponse> GetTransferTransaction(TransferTransactionRequest request);

    }
}
