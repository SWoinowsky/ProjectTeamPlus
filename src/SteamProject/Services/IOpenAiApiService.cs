namespace SteamProject.Services
{
    public interface IOpenAiApiService
    {
        public Task<string> SummarizeNewsShortAsync(string inputString);

        public Task<string> SummarizeNewsLongAsync(string inputString);


    }
}
