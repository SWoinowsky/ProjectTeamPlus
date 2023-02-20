using SteamProject.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
// using System.Text.Json;
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
            if(jsonResponse == null)
                return null;
            else
            {
                var poco = JsonConvert.DeserializeObject<LibraryPOCO>(jsonResponse);
                var games = new List<Game>();
                foreach(var game in poco.response.games)
                {
                    var temp = new Game();
                    temp = temp.TakeLibraryInfoPOCO(game, userId);
                    games.Add(temp);
                }
                return games.OrderBy(g => g.Name);
            }          
        }

        public Game GetGameDescription(Game game)
        {
            string source = string.Format("https://store.steampowered.com/api/appdetails?appids={0}", game.AppId);
            string jsonResponse = GetJsonStringFromEndpoint(source);
            
            if(jsonResponse != null)
            {   
                var regex = new Regex(Regex.Escape(game.AppId.ToString()));
                jsonResponse = regex.Replace(jsonResponse, "response", 1);
                var poco = JsonConvert.DeserializeObject<GameInfoPOCO>(jsonResponse);
                game.TakeGameInfoPOCO(poco);
            }
            return game;
        }


        public AchievementRoot GetAchievements(string userSteamId, int appId)
        {
            string source = string.Format("http://api.steampowered.com/ISteamUserStats/GetPlayerAchievements/v1/?appid={0}&key={1}&steamid={2}&l=en", appId, SteamToken, userSteamId);
            string response = GetJsonStringFromEndpoint(source);
            if (response == null)
            {
                return null;
            }
            // IEnumerable<Achievement> userAchievements = JsonSerializer.Deserialize<IEnumerable<Achievement>>(response)!;
            AchievementRoot deserialized = JsonConvert.DeserializeObject<AchievementRoot>(response)!;
            return deserialized;
        }

        public SchemaRoot GetSchema(int appId)
        {
            string source = string.Format("https://api.steampowered.com/ISteamUserStats/GetSchemaForGame/v2/?appid={0}&key={1}&l=en", appId, SteamToken);
            string response = GetJsonStringFromEndpoint(source);
            if (response == null)
            {
                return null;
            }
            // IEnumerable<Achievement> userAchievements = JsonSerializer.Deserialize<IEnumerable<Achievement>>(response)!;
            SchemaRoot deserialized = JsonConvert.DeserializeObject<SchemaRoot>(response)!;
            return deserialized;
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