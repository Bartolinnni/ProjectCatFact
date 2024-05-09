using NetwiseProject.Models;

namespace NetwiseProject.Services
{
    public interface ICatFactsClient
    {
        public Task<CatFact> DownloadCatFactAsync();
    }
}