using SteamProject.Models;
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

    public User SteamUser()
    {
        throw new NotImplementedException();
    }
}