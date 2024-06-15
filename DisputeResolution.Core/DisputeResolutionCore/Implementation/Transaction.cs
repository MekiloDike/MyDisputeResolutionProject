using Azure.Core;
using DisputeResolutionCore.Dto;
using DisputeResolutionCore.Interface;
using DisputeResolutionInfrastructure.HttpServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
namespace DisputeResolutionCore.Implementation
{
    public class Transaction : ITransaction
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Transaction> _logger;
        private readonly IHttpClientService _httpClientService;

        public Transaction(IConfiguration config, ILogger<Transaction> logger, IHttpClientService httpClientService)
        {
            _config = config;
            _logger = logger;
            _httpClientService = httpClientService;
        }
        public async Task<TokenResponse> GetAccessToken()
        {
            var result = await _httpClientService.GetToken<TokenResponse>();
            return result;

        }
        public async Task<AgencyBankingResponse> GetAgencyBanking(AgencyBankingRequest request)
        {
            var agenyResponse = new AgencyBankingResponse();
            try
            {
                var baseUrl = _config["Dispute:baseUrl"];

                // _logger.LogInformation("Http call to {url} to get token", url);
                // Define the request parameters
                var date = DateConvert(request.date);

                var startDate = date.startDate;
                string endDate = date.endDate;
                var terminal = request.terminal;
                var pan = request.pan;
                var stan = request.stan;

                var urlWithParams = $"{baseUrl}/api/v1/transactions/groups/AGENCY BANKING/channels/ALL?startDate={startDate}&endDate={endDate}&terminal={terminal}&pan={pan}&stan={stan}";
                var tokenRespone = await GetAccessToken();
                var token = tokenRespone.access_token;
                var domain = tokenRespone.client_authorization_domain;

                agenyResponse = await _httpClientService.GetTransaction<AgencyBankingResponse>(urlWithParams, token, domain);
                return agenyResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //log the execption;
                _logger.LogError("An error occured: {error}", ex.Message);
                return agenyResponse;
            }

        }
        public async Task<IpgTransactionResponse> GetIpgTransaction(IpgTransactionRequest request)
        {
            var baseUrl = _config["Dispute:baseUrl"];

            // _logger.LogInformation("Http call to {url} to get token", url);
            // Define the request parameters
            var date = DateConvert(request.date);

            var startDate = date.startDate;
            string endDate = date.endDate;
            var stan = request.stan;
            var maskedCardPan = request.maskedCardPan;
            var retrievalReferenceNumber = request.retrievalReferenceNumber;
            var merchantCode = request.merchantCode;

            var urlWithParams = $"{baseUrl}/api/v1/transactions/groups/AGENCY BANKING/channels/ALL?startDate={startDate}&endDate={endDate}&maskedCardPan={maskedCardPan}&retrievalReferenceNumber={retrievalReferenceNumber}&stan={stan}&merchantCode={merchantCode}";
            var tokenRespone = await GetAccessToken();
            var token = tokenRespone.access_token;
            var domain = tokenRespone.client_authorization_domain;

            var ipgResponse = await _httpClientService.GetTransaction<IpgTransactionResponse>(urlWithParams, token, domain);
            return ipgResponse;
        }



        //create a method to return the beginning of the day given a date
        public (string startDate, string endDate) DateConvert(DateTime inputDate) //return as tuple
        {
            // Start of the day (midnight)
            string startDateTime = new DateTime(inputDate.Year, inputDate.Month, inputDate.Day, 0, 0, 0).ToString();

            // End of the day (23:59:59)
            string endDateTime = new DateTime(inputDate.Year, inputDate.Month, inputDate.Day, 23, 59, 59).ToString();
            return (startDateTime, endDateTime);
        }


        public async Task<TransferTransactionResponse> GetTransferTransaction(TransferTransactionRequest transferRequest)
        {
            var transferResult = new TransferTransactionResponse();
            try
            {
                var baseUrl = _config["Dispute:baseUrl"];

                // _logger.LogInformation("Http call to {url} to get token", url);
                // Define the request parameters
                var date = DateConvert(transferRequest.date);

                var startDate = date.startDate;
                string endDate = date.endDate;
                var terminal = transferRequest.terminal;
                var pan = transferRequest.pan;
                var stan = transferRequest.stan;

                var urlWithParams = $"{baseUrl}/api/v1/transactions/groups/TRANSFERS/channels/ALL?startDate={startDate}&endDate={endDate}&terminal={terminal}&pan={pan}&stan={stan}";
                var tokenRespone = await GetAccessToken();
                var token = tokenRespone.access_token;
                var domain = tokenRespone.client_authorization_domain;

                transferResult = await _httpClientService.GetTransaction<TransferTransactionResponse>(urlWithParams, token, domain);
                return transferResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //log the execption;
                _logger.LogError("An error occured: {error}", ex.Message);
                return transferResult;
            }

        }
    }


}

