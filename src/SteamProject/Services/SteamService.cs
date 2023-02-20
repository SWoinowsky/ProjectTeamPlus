using SteamProject.Models;
using SteamProject.Models.DTO;
using System.Text.Json;


namespace SteamProject.Services;

public class SteamService : ISteamService
{
    public static readonly HttpClient _httpClient = new HttpClient();
    string Token;

    
    public SteamService( string token )
    {
        Token = token;
    }

    public string GetJsonStringFromEndpoint(string uri)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage( HttpMethod.Get, uri );

        var response = client.Send(request);

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

    public User GetSteamUser(string steamid)
    {
        string uri = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={Token}&steamids={steamid}";
        string? jsonResponse = GetJsonStringFromEndpoint( uri );

        var poco = JsonSerializer.Deserialize<SteamUserPOCO>(jsonResponse);

        var returnMe = new User();
        returnMe.TakeSteamPOCO(poco);

        return returnMe;
    }

    public int GetUserLevel(string steamid)
    {
        string uri = $"https://api.steampowered.com/IPlayerService/GetSteamLevel/v1/?key={Token}&steamid={steamid}";
        string? jsonResponse = GetJsonStringFromEndpoint( uri );

        var returnMe = JsonSerializer.Deserialize<UserLevelPOCO>(jsonResponse).response.player_level;

        return returnMe;
    }
}