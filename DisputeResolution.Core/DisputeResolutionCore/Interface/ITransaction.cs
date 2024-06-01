using DisputeResolutionCore.Dto;

namespace DisputeResolutionCore.Interface
{
    public interface ITransaction
    {
        //get token
        Task<TokenResponse> GetAccessToken();
        //get 3 different transactionbuilder.Services.AddScoped<IUserManagement, UserManagement>();
        Task<IpgTransactionResponse> GetIpgTransaction(IpgTransactionRequest request);
        Task<AgencyBankingResponse> GetAgencyBankin(AgencyBankingRequest request);
        Task<TransferTransactionResponse> GetIpTransaction(TransferTransactionRequest request);

    }
}
