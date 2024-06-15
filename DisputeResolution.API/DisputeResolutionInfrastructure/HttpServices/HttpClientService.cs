using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace DisputeResolutionInfrastructure.HttpServices
{
    public class HttpClientService : IHttpClientService
    {

        private readonly IConfiguration _config;
        private readonly ILogger<HttpClientService> _logger;
        public HttpClientService(IConfiguration config,ILogger<HttpClientService> logger)
        {
            _config = config;
            _logger = logger;
        }
        public async Task<T?> GetTransaction<T>(string urlWithParams, string token, string domain) where T : class
        {
            T? result = default;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var httpManHandler = new HttpClientHandler();
            // httpManHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpManHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Create the HttpClient
            using (HttpClient client = new HttpClient(httpManHandler) { Timeout = TimeSpan.FromSeconds(60) })
            {
                // Set up Basic Authentication
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
                    result = JsonConvert.DeserializeObject<T>(responseContent);
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

        public async Task<T> GetToken<T>()
        {
            T result = default;

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
                    result = JsonConvert.DeserializeObject<T>(responseContent);
                    Console.WriteLine("Response: " + responseContent);
                    _logger.LogInformation($"Http Call TokenResponse: {JsonConvert.SerializeObject(result)}");
                    return result;
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                    return result;
                }
            }
        }
    }
}
