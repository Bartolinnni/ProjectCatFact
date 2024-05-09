using Microsoft.AspNetCore.Mvc;
using NetwiseProject.Services;
using System.Net;

namespace NetwiseProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatFactsController : ControllerBase
    {
        private readonly ILogger<CatFactsController> _logger;
        private readonly ICatFactsClient _catFactsClient;

        public CatFactsController(ILogger<CatFactsController> logger,
            ICatFactsClient catFactsClient)
        {
            _logger = logger;
            _catFactsClient = catFactsClient;
        }

        [HttpGet(Name = "GetCatFact")]
        public async Task<IActionResult> GetCatFact()
        {
            _logger.LogInformation("Downloading catFact");
            var fact = await _catFactsClient.DownloadCatFactAsync();

            if (fact == null)
            {
                _logger.LogError("Failed to download a CatFact from the external API.");
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, "The cat facts service is currently unavailable. Please try again later.");
            }

            string filePath = "catfacts.txt";
            _logger.LogInformation("Saving catFact in local txt file");
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(fact.Fact);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when writing to the file: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while saving the cat fact. Please try again later.");
            }

            return Ok(fact);
        }

    }
}
