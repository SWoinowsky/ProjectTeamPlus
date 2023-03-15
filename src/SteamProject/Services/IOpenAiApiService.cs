namespace SteamProject.Services
{
    public interface IOpenAiApiService
    {
        public Task<string> SummarizeTextAsync(string inputString);

    }
}
