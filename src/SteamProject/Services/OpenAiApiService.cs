using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace SteamProject.Services
{
    public class OpenAiApiService : IOpenAiApiService
    {
        private readonly OpenAIService _openAiService;
        public OpenAiApiService (string token)
        {
            _openAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = token
            });
        }

        public async Task<string> SummarizeTextAsync(string text)
        {
            var completionResult = await _openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("You are a helpful assistant meant to summarize patch notes and video game news in a max of two sentences, unless its a list of games then those are the top selling things on steam"),
                    ChatMessage.FromUser(text),
                },
                Model = OpenAI.GPT3.ObjectModels.Models.ChatGpt3_5Turbo
            });
            if (completionResult.Successful)
            {
                return completionResult.Choices.First().Message.Content;
            }

            return null;
        }

    }
}
