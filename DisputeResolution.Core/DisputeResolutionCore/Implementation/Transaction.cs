using DisputeResolutionCore.Dto;
using DisputeResolutionCore.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace DisputeResolutionCore.Implementation
{
    public class Transaction : ITransaction
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Transaction> _logger;
        public Transaction(IConfiguration config, ILogger<Transaction> logger)
        {
            _config = config;
            _logger = logger;
        }
        public async Task<TokenResponse> GetAccessToken()
        {
            var result = new TokenResponse();
            try
            {
                var url = _config["AccessToken:url"];
                var grant_type = _config["AccessToken:grantType"];
                var scope = _config["AccessToken:scope"];
                var username = _config["AccessToken:clientId"];
                var password = _config["AccessToken:clientSecret"];

                _logger.LogInformation("Http call to {url} to get token", url);
                // Define the form data
                var requestBody = new Dictionary<string, string>
               {
                     { "grant_type", $"{grant_type}" },
                     { "scope", $"{scope}" }
               };

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpManHandler = new HttpClientHandler();
                // httpManHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
                httpManHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Create the HttpClient
                using (HttpClient client = new HttpClient(httpManHandler) { Timeout = TimeSpan.FromSeconds(60) })
                {
                    // Set up Basic Authentication
                    var authToken = Encoding.ASCII.GetBytes($"{username}:{password}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                    // Convert the form data to x-www-form-urlencoded content
                    HttpContent content = new FormUrlEncodedContent(requestBody);

                    // Send the POST request
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Check the response status
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        string responseContent = await response.Content.ReadAsStringAsync();
                        //deserialize to the response object
                        result = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                        Console.WriteLine("Response: " + responseContent);
                        return result;
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //log the execption;
                _logger.LogError("An error occured: {error}", ex.Message);
                return result;
            }

        }
        public async Task<AgencyBankingResponse> GetAgencyBankin(AgencyBankingRequest request)
        {
            var result = new AgencyBankingResponse();
            try
            {
                var baseUrl = _config["Dispute:baseUrl"];

                // _logger.LogInformation("Http call to {url} to get token", url);
                // Define the request parameters
                var startDate = request.date;
                string endDate = request.date.ToString();
                var terminal = request.terminal;
                var pan = request.pan;
                var stan = request.stan;

                var urlWithParams = $"{baseUrl}/api/v1/transactions/groups/TRANSFERS/channels/ALL?startDate={startDate}&endDate={endDate}&terminal={terminal}&pan={pan}&stan={stan}";
                var tokenRespone = await GetAccessToken();
                var token = tokenRespone.access_token;
                var domain = tokenRespone.client_authorization_domain;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpManHandler = new HttpClientHandler();
                // httpManHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
                httpManHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Create the HttpClient
                using (HttpClient client = new HttpClient(httpManHandler) { Timeout = TimeSpan.FromSeconds(60) })
                {
                    // Set up Basic Authentication
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
                    // Add custom header
                    client.DefaultRequestHeaders.Add("X-Interswitch-Authorization-Domain", domain);

                    // Send the Get request
                    HttpResponseMessage response = await client.GetAsync(urlWithParams);

                    // Check the response status
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        string responseContent = await response.Content.ReadAsStringAsync();
                        //deserialize to the response object
                        result = JsonConvert.DeserializeObject<AgencyBankingResponse>(responseContent);
                        Console.WriteLine("Response: " + responseContent);
                        return result;
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //log the execption;
                _logger.LogError("An error occured: {error}", ex.Message);
                return result;
            }

        }

        public Task<IpgTransactionResponse> GetIpgTransaction(IpgTransactionRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<TransferTransactionResponse> GetTransferTransaction(TransferTransactionRequest request)
        {
            throw new NotImplementedException();
        }

        //create a method to return the beginning of the day given a date

        public class DisputeDate
        {
            public string StartDate { get; set; }
            public string EndDate { get; set; }
        }

        public DisputeDate DateConverter(DateTime dateTime)
        {
            // Start of the day (midnight)
            DateTime startDateTime = dateTime.Date;

            // End of the day (last moment of the day)
            DateTime endDateTime = startDateTime.AddDays(1).AddTicks(-1);

            return new DisputeDate()
            {
                StartDate = startDateTime.ToString(),
                EndDate = endDateTime.ToString(),
            };

        }
    }
}

