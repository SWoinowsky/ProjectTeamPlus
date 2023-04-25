using Microsoft.AspNetCore.Identity;
using SteamProject.Models;
using SteamProject.Models.DTO;
using SteamProject.Helpers;
using System.Text.Json;
using System.Text.RegularExpressions;
using SteamProject.ViewModels;
using AngleSharp.Dom;

namespace SteamProject.Services;

public class SteamService : ISteamService
{
    public static readonly HttpClient _httpClient = new HttpClient();
    string Token;
    string AdminToken;

    
    public SteamService( string token, string adminToken )
    {
        Token = token;
        AdminToken = adminToken;
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

    public List<User> GetManyUsers( List<string> steamIds )
    {
        string uri = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={Token}&steamids=";

        foreach( string id in steamIds )
        {
            uri += id;
            uri += ",";
        }

        string? jsonResponse = GetJsonStringFromEndpoint( uri );

        var poco = JsonSerializer.Deserialize<SteamUserPOCO>(jsonResponse);

        var userList = new List<User>();
        foreach( var id in steamIds )
        {
            var userOut = new User();
            userOut.TakeSteamPOCO( poco );
            userList.Add(userOut);

            poco.response.players.RemoveAt( 0 );
        }

        return userList;
    }

    public int GetUserLevel(string steamid)
    {
        string uri = $"https://api.steampowered.com/IPlayerService/GetSteamLevel/v1/?key={Token}&steamid={steamid}";
        string? jsonResponse = GetJsonStringFromEndpoint( uri );

        var returnMe = JsonSerializer.Deserialize<UserLevelPOCO>(jsonResponse).response.player_level;

        return returnMe;
    }

    public List<Friend> GetFriendsList(string steamid, int userId)
    {
        string friendsListUri = $"https://api.steampowered.com/ISteamUser/GetFriendList/v1/?key={Token}&steamid={steamid}";
        string? jsonResponse = GetJsonStringFromEndpoint( friendsListUri );

        
        var friendPocoList = JsonSerializer.Deserialize<FriendsListPOCO>(jsonResponse).friendslist.friends;

        var idList = new List<string>();
        foreach( var friend in friendPocoList )
        {
            idList.Add( friend.steamid );
        }

        string friendIdsParam = "";
        foreach( var id in idList )
        {
            friendIdsParam += $"{id},";
        }

        string friendsInfoUri = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={Token}&steamids={friendIdsParam}";
        jsonResponse = GetJsonStringFromEndpoint( friendsInfoUri );

        var FriendsData = JsonSerializer.Deserialize<SteamUserPOCO>(jsonResponse).response.players;
        var FriendsList = new List<Friend>();
        foreach( var friend in FriendsData )
        {
            var FriendOut = new Friend();
            FriendOut.TakePlayerPOCO(friend);
            FriendOut.RootId = userId;
            FriendsList.Add(FriendOut);
        }

        return FriendsList;
    }

    public Friend GetFriendSpecific( string userSteamId, int userId, string friendSteamId )
    {
        var returnMe = GetFriendsList(userSteamId, userId).Where( f => f.SteamId == friendSteamId ).FirstOrDefault();

        return returnMe;
    }

    public IEnumerable<Game> GetGames(string userSteamId, int userId)
    {
        string source = string.Format("http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={0}&steamid={1}&format=json&include_appinfo=1", Token, userSteamId);
        string jsonResponse = GetJsonStringFromEndpoint(source);
        if(jsonResponse == null)
            return null;
        else
        {
            var poco = JsonSerializer.Deserialize<LibraryPOCO>(jsonResponse);
            var games = new List<Game>();
            if( poco.response.games is not null )
            {
                if( poco.response.games.Count() != 0 )
                {
                    foreach(var game in poco.response.games)
                    {
                        var temp = new Game();
                        temp = temp.TakeLibraryInfoPOCO(game, userId);
                        games.Add(temp);
                    }
                }
            }
            return games.OrderBy(g => g.Name);
        }
    }

    public GameVM GetGameInfo(Game game)
    {
        var gameVM = new GameVM();
        string source = string.Format("https://store.steampowered.com/api/appdetails?appids={0}&l=en", game.AppId);
        string jsonResponse = GetJsonStringFromEndpoint(source);
        
        if(jsonResponse != null)
        {   
            var regex = new Regex(Regex.Escape(game.AppId.ToString()));
            jsonResponse = regex.Replace(jsonResponse, "response", 1);
            try
            {
                regex = new Regex(Regex.Escape("linux_requirements\":[]"));
                jsonResponse = regex.Replace(jsonResponse, "linux_requirements\":{}", 1);
            }
            catch {}
            try
            {
                regex = new Regex(Regex.Escape("mac_requirements\":[]"));
                jsonResponse = regex.Replace(jsonResponse, "mac_requirements\":{}", 1);
            }
            catch {}
            try
            {
                regex = new Regex(Regex.Escape("pc_requirements\":[]"));
                jsonResponse = regex.Replace(jsonResponse, "pc_requirements\":{}", 1);
            }
            catch {}
            gameVM._poco = JsonSerializer.Deserialize<GameInfoPOCO>(jsonResponse);
        }
        return gameVM;
    }



    public GameNewsVM GetGameNews(Game game, int count = 10)
    {
        var gameVM = new GameNewsVM();
        int fetchCount = 2+count * 2; // Fetch more items to account for the SteamDb items
        string source = string.Format("https://api.steampowered.com/ISteamNews/GetNewsForApp/v0002/?appid={0}&count={1}&l=en", game.AppId, fetchCount);
        string jsonResponse = GetJsonStringFromEndpoint(source);

        if (jsonResponse != null)
        {
            var newsPoco = JsonSerializer.Deserialize<GameNewsPoco>(jsonResponse);
            var filteredNewsItems = new List<Newsitem>();

            for (var i = 0; i < newsPoco.appnews.newsitems.Count && filteredNewsItems.Count < count; i++)
            {
                var newsItem = newsPoco.appnews.newsitems[i];

                if (newsItem.feedlabel.ToUpper() != "SteamDb".ToUpper())
                {
                    newsItem.contents = HelperMethods.StripJunkFromString(newsItem.contents);
                    newsItem.dateTime = HelperMethods.UnixTimeStampToDateTime(newsItem.date);
                    filteredNewsItems.Add(newsItem);
                }
                
            }

            // Limit the results to the requested count
            newsPoco.appnews.newsitems = filteredNewsItems.Take(count).ToList();
            gameVM._poco = newsPoco;
        }

        return gameVM;
    }

    public AchievementRoot GetAchievements(string userSteamId, int appId)
    {
        string source = string.Format("http://api.steampowered.com/ISteamUserStats/GetPlayerAchievements/v1/?appid={0}&key={1}&steamid={2}&l=en", appId, Token, userSteamId);
        string response = GetJsonStringFromEndpoint(source);
        if (response == null)
        {
            return null;
        }
        AchievementRoot deserialized = JsonSerializer.Deserialize<AchievementRoot>(response)!;
        return deserialized;
    }

    public SchemaRoot GetSchema(int appId)
        {
            string source = string.Format("https://api.steampowered.com/ISteamUserStats/GetSchemaForGame/v2/?appid={0}&key={1}&l=en", appId, Token);
            string response = GetJsonStringFromEndpoint(source);
            if (response == null)
            {
                return null;
            }
            // IEnumerable<Achievement> userAchievements = JsonSerializer.Deserialize<IEnumerable<Achievement>>(response)!;
            SchemaRoot deserialized = JsonSerializer.Deserialize<SchemaRoot>(response)!;
            return deserialized;
        }

    public GAPRoot GetGAP(int appId)
    {
        string source = string.Format("https://api.steampowered.com/ISteamUserStats/GetGlobalAchievementPercentagesForApp/v0002/?gameid={0}&format=json", appId);
        string response = GetJsonStringFromEndpoint(source);
        if (response == null)
        {
            return null;
        }
        GAPRoot deserialized = JsonSerializer.Deserialize<GAPRoot>(response)!;
        return deserialized;
    }
}