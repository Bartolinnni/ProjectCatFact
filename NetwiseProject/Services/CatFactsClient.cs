using NetwiseProject.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace NetwiseProject.Services
{
    public class CatFactsClient : ICatFactsClient
    {
        private readonly ILogger<CatFactsClient> _logger;
        private readonly String ApiUrl = "https://catfact.ninja";
        private readonly RestClient _client;
        
        public CatFactsClient(ILogger<CatFactsClient> logger)
        {
            _logger = logger;
            _client = new RestClient(new Uri(ApiUrl));
        }

        public async Task<CatFact> DownloadCatFactAsync()
        {
            _logger.LogInformation("Requesting a new cat fact from the API.");
            var request = new RestRequest("/fact");
            var response = await _client.ExecuteAsync(request);

            if (response == null || response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError("Failed to download a cat fact from the API. Response Content = {@response?.Content}", response?.Content);
                return null;
            }

            _logger.LogInformation("Deserializing and returning the cat fact response.");
            var fact = JsonConvert.DeserializeObject<CatFact>(response.Content);

            return fact;
        }
    }
}
