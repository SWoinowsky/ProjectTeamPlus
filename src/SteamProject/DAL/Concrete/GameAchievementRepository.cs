using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;
using SteamProject.Models.DTO;
using SteamProject.Services;

namespace SteamProject.DAL.Concrete;

public class GameAchievementRepository : Repository<GameAchievement>,  IGameAchievementRepository
{

    private readonly SteamInfoDbContext _ctx;
    private readonly IGameRepository _gameRepository;
    private readonly ISteamService _steamService;  

    public GameAchievementRepository(SteamInfoDbContext ctx, IGameRepository gameRepository, ISteamService steamService) : base(ctx)
    {
        _gameRepository = gameRepository;
        _steamService = steamService;
    }

    public List<GameAchievement> GetAchievementsFromGameId( int gameId )
    {
        return GetAll().Where( gA => gA.GameId == gameId ).ToList();
    }

    public void EnsureGameAchievements(int appId, string steamId, int userId)
    {
        var gameIsInDb = _gameRepository.GetGameByAppId(appId) != null;
        if (!gameIsInDb)
        {
            var gamesToAdd = _steamService.GetGames(steamId, userId).ToList<Game>();
            foreach (var game in gamesToAdd)
            {
                _gameRepository.AddOrUpdate(game);
            }
        }

        var gameIdFound = _gameRepository.GetGameByAppId(appId).Id;

        var gameAchievements = GetAchievementsFromGameId(gameIdFound);
        if (gameAchievements.Count() == 0)
        {
            var rawAchievements = _steamService.GetSchema(appId).game.availableGameStats.achievements.ToList<SchemaAchievement>();

            foreach (var rawAch in rawAchievements)
            {
                var addMe = new GameAchievement(rawAch);
                addMe.GameId = gameIdFound;
                gameAchievements.Add(addMe);
                AddOrUpdate(addMe);
            }
        }
    }

}