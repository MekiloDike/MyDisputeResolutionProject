using DisputeResolutionCore.Interface;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DisputeResolutionInfrastructure.HttpServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DisputeResolutionCore.Dto;
using static System.Formats.Asn1.AsnWriter;
using DisputeResolutionCore.Utility;
using static DisputeResolutionCore.Dto.CreateDisputeResponse;

namespace DisputeResolutionCore.Implementation
{
    public class Dispute : IDispute
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientService _httpClientService;
        public Dispute(IConfiguration config, IHttpClientService httpClientService)
        {
            _config = config;
            _httpClientService = httpClientService;
        }

        public async Task CreateDispute(CreateDisputeRequest request)
        {
            try 
            {
                using var client = new HttpClient();
                var baseUrl = _config["Dispute:url"];
                var url = baseUrl + "/api/v1/disputes";
                var tokenService = new TokenService(_httpClientService);
                var tokenResponse = await tokenService.GetAccessToken();
                var token = tokenResponse.access_token;
                var domain = tokenResponse.client_authorization_domain;

                // Serialize the body to JSON
                var jsonBody = JsonConvert.SerializeObject(request);

                // Create the HttpContent
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                // Add authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("X-Interswitch-Authorization-Domain", domain);


                // Send the POST request
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Ensure the request was successful or throw an exception
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                var responseContent = await response.Content.ReadAsStringAsync();

                //deserialize to the response object
                var result = JsonConvert.DeserializeObject(responseContent);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            


        }

        public async Task<GetDisputeResponse> GetDispute(string logCode)
        {
            var result = new GetDisputeResponse();
            try
            {
                var baseUrl = _config["Dispute:url"];
                var url = baseUrl + $"/api/v1/disputes/{logCode}";
                var tokenService = new TokenService(_httpClientService);
                var tokenResponse = await tokenService.GetAccessToken();
                var token = tokenResponse.access_token;
                var domain = tokenResponse.client_authorization_domain;
                using var client = new HttpClient();

                // Add the authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("X-Interswitch-Authorization-Domain", domain);


                // Send the GET request and get the response
                HttpResponseMessage response = await client.GetAsync(url);

                // Ensure the request was successful or throw an exception
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string responseContent = await response.Content.ReadAsStringAsync();
                 result = JsonConvert.DeserializeObject<GetDisputeResponse>(responseContent);

            }
             catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
                return result;

        }
    }
}