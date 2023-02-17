using SteamProject.Models;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using SteamProject.Models.DTO;
using System.Text.RegularExpressions;

namespace SteamProject.Services
{
    public class SteamServices : ISteamServices
    {
        public string SteamToken;

        public void SetCredentials(string token)
        {
            SteamToken = token;
        }

        public SteamServices (string token)
        {
            SetCredentials(token);
        }

        public IEnumerable<Game> GetGames(string userSteamId, int userId, User user)
        {
            string source = string.Format("http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={0}&steamid={1}&format=json&include_appinfo=1", SteamToken, userSteamId);
            string jsonResponse = GetJsonStringFromEndpoint(source);
            var slicedResponse = jsonResponse[jsonResponse.IndexOf("[")..^2];
            var games = new List<Game>();
            JArray gamesArray = JArray.Parse(slicedResponse);
            
            for(int i = 0; i < gamesArray.Count; i++)
            {
                var tempGame = new Game();
                tempGame.FromJson(gamesArray[i].ToString(), userId, user);
                tempGame.DescShort = "";
                tempGame.DescLong = "";
                games.Add(tempGame);
            }
            return games.OrderBy(g => g.Name);
        }

        public Game GetGameDescription(Game game)
        {
            string source = string.Format("https://store.steampowered.com/api/appdetails?appids={0}", game.AppId);
            string jsonResponse = GetJsonStringFromEndpoint(source);
            
            if(jsonResponse != null)
            {   
                var regex = new Regex(Regex.Escape(game.AppId.ToString()));
                jsonResponse = regex.Replace(jsonResponse, "response", 1);
                var poco = JsonSerializer.Deserialize<GameInfoPOCO>(jsonResponse);
                game.TakeGameInfoPOCO(poco);
            }
            return game;
        }

        // This is a singleton, we are only supposed to have one per application
        public static readonly HttpClient _httpClient = new HttpClient();

        public string GetJsonStringFromEndpoint(string uri)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage response = _httpClient.Send(httpRequestMessage);
            // This is only a minimal version; make sure to cover all your bases here
            if (response.IsSuccessStatusCode)
            {
                // Note there is only an async version of this so to avoid forcing you to use all async I'm waiting for the result manually
                string responseText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return responseText;
            }
            else
            {
                // What to do if failure? 401? Should throw and catch specific exceptions that explain what happened
                return null;
            }
        }
    }
}