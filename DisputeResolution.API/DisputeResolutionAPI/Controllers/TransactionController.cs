using DisputeResolutionCore.Dto;
using DisputeResolutionCore.Interface;
using Microsoft.AspNetCore.Mvc;


namespace DisputeResolutionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransaction _transaction;
        private readonly ILogger<TransactionController> _logger;
        public TransactionController(ITransaction transaction, ILogger<TransactionController> logger)
        {
            _transaction = transaction;
            _logger = logger;
        }

        [HttpPost("token")]
        // [Route("token")]
        public async Task<IActionResult> GetAccessTokenAsync()
        {
            _logger.LogInformation("TransactionController: About to get access token");
            var result = await _transaction.GetAccessToken();

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

    }
        
}
