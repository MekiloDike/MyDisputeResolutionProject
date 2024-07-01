using DisputeResolutionCore.Dto;
using DisputeResolutionCore.Interface;
using DisputeResolutionCore.Utility;
using DisputeResolutionInfrastructure.Entity;
using DisputeResolutionInfrastructure.HttpServices;
using Microsoft.AspNetCore.Mvc;


namespace DisputeResolutionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransaction _transaction;
        private readonly ILogger<TransactionController> _logger;
        private readonly IHttpClientService _httpClientService;
        private readonly IDispute _Dispute;
        private readonly ILogTransaction _logTransaction;
        public TransactionController(ITransaction transaction, ILogger<TransactionController> logger, IHttpClientService httpClientService, IDispute dispute, ILogTransaction logTransaction)
        {
            _transaction = transaction;
            _logger = logger;
            _httpClientService = httpClientService;
            _Dispute = dispute;
            _logTransaction = logTransaction;
        }

        [HttpPost("token")]
        // [Route("token")]
        public async Task<IActionResult> GetAccessTokenAsync()
        {
            _logger.LogInformation("TransactionController: About to get access token");
            var tokenService = new TokenService(_httpClientService);
            var result = await tokenService.GetAccessToken();

            _logger.LogInformation("TransactionController: Access token gotten");
            return Ok(result);
        }



        [HttpPost("Agency-Banking")]
        public async Task<IActionResult> GetAgencyBankin(AgencyBankingRequest agencyBanking)
        {
            var result = await _transaction.GetAgencyBanking(agencyBanking);
            return Ok(result);

        }

        [HttpPost("Ipg-Transaction")]
        public async Task<IActionResult> GetIpgTransaction(IpgTransactionRequest request)
        {
            var result = await _transaction.GetIpgTransaction(request);
            return Ok(result);

        }

        [HttpPost("Transfer-Transaction")]
        public async Task<IActionResult> GetTransferTransaction(TransferTransactionRequest transferRequest)
        {
            var result = await _transaction.GetTransferTransaction(transferRequest);
            return Ok(result);

        }

        [HttpPost("Create-TransactionLog")]
        public async Task<IActionResult> CreateTransactionLog(DisputeRequestLogDto Request)
        {
            var result = await _logTransaction.CreateTransactionLog(Request);
            return Ok(result);

        }

        [HttpGet("Get-Logged-Transaction")]
        public async Task<IActionResult> GetLoggedTransaction(string transactionReference)
        {
            var result = await _logTransaction.GetLoggedTransaction(transactionReference);
            return Ok(result);


        }
    }
        
}
